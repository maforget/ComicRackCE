# OtherScripts.py
#
# Includes some sample scripts:
#   SaveCSVList - Writes some book properties as a csv file.
#   RenameBookFiles - Renames the book files to a default name pattern
#
# You are free to modify and distribute this file
#
##########################################################################

import clr
clr.AddReferenceByPartialName("System.Windows.Forms")
from System.Windows.Forms import *

import sys
sys.setdefaultencoding("utf8")

#
# Simple SaveFileDialog
#
def GetFileName(comicList):
	dlg = SaveFileDialog()
	dlg.Filter = "CSV Files (*.csv)|*.csv"
	dlg.FileName = comicList
	dlg.DefaultExt = ".csv"
	dlg.CheckPathExists = True
	if dlg.ShowDialog(ComicRack.MainWindow) == DialogResult.Cancel:
		return ""
	else:
		return dlg.FileName

#
# Saves a CSV list of all the passed comics to a selectable file
#
#@Name	Export Book List...
#@Hook	Books, Library
#@Description Simple script to export all selected Books into a CSV file
def SaveCSVList(books):
	name = GetFileName("Book List")
	if name=="":
		return
	f=open(name, "w")
	for book in books:
		print book.ShadowSeries
		f.write (book.ShadowSeries + ";")
		f.write (book.ShadowTitle + ";")
		f.write (book.ShadowNumber + ";")
		f.write (str(book.ShadowVolume) + ";")
		f.write (str(book.ShadowYear) + "\n")
	f.close()

#
# Rename Book Files to 'Series Volume #Number (of Count) (Year)'
# Some processing is done to correctly handle missing parameters
#
#@Name Rename Files to 'Series Volume #Number (of Count) (Year)'
#@Hook Books
#@Description Script to rename the selected Books based on the meta data
def RenameBookFiles(books):
	for book in books:
		name = book.ShadowSeries
		if name != "":
			if book.ShadowVolume != -1:
				name = name + " V" + str(book.ShadowVolume)
			if book.ShadowNumber != "":
				name = name + " #" + book.ShadowNumber
				if book.ShadowCount != -1:
					name = name + " (of " + str(book.ShadowCount) + ")"
			if book.ShadowYear != -1:
				name = name + " (" + str(book.ShadowYear) + ")"
			book.RenameFile (name)

#
# Marks the Book for update of info to file
#
#@Name Mark Info as dirty
#@Hook Books
#@Description Marks the info as dirty
#@Enabled false
def SetComicInfoDirty(books):
	for b in books:
		b.ComicInfoIsDirty = True
	