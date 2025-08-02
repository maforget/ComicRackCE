# -*- coding: utf-8 -*-
#@Name FromDucks
#@Hook Books
#@Image fromducks.ico
#@Key FromDucks
#@Description Search on inducks.org informations about the selected eComics
#
# FromDucks 2.16 - Jul 2025
#
# ---> 2.16 Changelog: Fix Inducks domain name, fix log function being passed wrongly typed parameters
#
# FromDucks 2.15 - Jun 2022
#
# ---> 2.15 Changelog: Fixed bugs found by /u/MxFlix. Packaged by /u/quinyd
#
# FromDucks 2.14 - Jul 2018
#
# ---> 2.14 Changelog: fixed other bugs
#
# You are free to modify, enhance (possibly) and distribute this file, but only if mentioning the (c) part
#
# (C)2007-2013 cYo Soft & (C)2008 DouglasBubbletrousers & (C)2008 wadegiles & (C)2010 cbanack
# (C)2009-2010 oraclexview aka SoundWave
#
# Data (C) The Inducks Team (http://inducks.org)
# The data presented here is based on information from the freely available Inducks database.
#
# This script is freely available to be modifiedy and redistributed
#
# !!!!!!!! BACKUP YOUR FILES BEFORE USING THIS SCRIPT !!!!!!!!! Yes, at least if you plan a mass scarping...
# Sorry guys, I cannot be held responsible if you screw your database...
#
# Some notes regarding the use of this script:
# Select 1 or more comics, same series; start the script and define the desired behavior.
# Pick the correct Series name for I.N.D.U.C.K.S., select which fields to fill:
# * a marked box means OVERRIDE ALWAYS *
# * a shaded box means OVERRIDE IF EMPTY *
# * a blank box means DO NOT OVERRIDE *
# On the Title label, clicking will change th way the title is written, from original to all initials capitalized
# Choose the genre if wanted, pick a translation language with a double click (characters' names will be set in that language, if existing)
# Click OK and wait...
# RESET will cycle the checkbox status
# REBUILD will rebuild/download the tables (tables are read once and then stored)
# CANCEL will abort the script
#
# Bugs, suggestions or comments in the Comicrack Forum !
#
#################################################################################################################

import re, sys

from datetime import datetime
settings = ""
VERSION = "2.16"

# DEBUG = False
DEBUG = True

fileHandle = 0

SIZE_RENAME_LOG = 100000
SIZE_DEBUG_LOG = 100000

def FromDucks(books):

	class ProgressBarDialog(Form):
		def __init__(self, nMax, cText="Scraping Files"):
			from System.Drawing import Point, Size, Color
			from System.Windows.Forms import (
				Label, FormStartPosition, ProgressBar
			)

			self.Text = cText #"Scraping Files"
			self.Size = Size(350, 150)
			self.StartPosition = FormStartPosition.CenterScreen

			self.pb = ProgressBar()
			self.traitement = Label()

			self.traitement.Location = Point(20, 5)
			self.traitement.Name = "scraping"
			self.traitement.Size = Size(300, 50)
			self.traitement.Text = ""

			self.pb.Size = Size(300, 20)
			self.pb.Location = Point(20, 50)
			self.pb.Maximum = 100
			self.pb.Minimum = 0
			self.pb.Step = 100.00 / nMax
			self.pb.Value = 0.00
			self.pb.BackColor = Color.LightGreen
			self.pb.Text = ""
			self.pb.ForeColor = Color.Black

			self.Controls.Add(self.pb)
			self.Controls.Add(self.traitement)

		def Update(self, cText, nInc, cHead="Scraping Files "):
			self.traitement.Text = "\n" + cText
			if nInc > 0:
				self.Text = cHead + self.pb.Value.ToString() + "%"
				self.pb.Increment(self.pb.Step)
	
	class BuilderForm(Form):

		def __init__(self):
			from System.Drawing import Point, Size, Color
			from System.Windows.Forms import (
				DialogResult, Button, ListBox, TextBox, Label, CheckBox, CheckState,
				FormBorderStyle, FormStartPosition
			)

			global aUpdate, Translation, LStart

			aUpdate = [2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,""]
			# series title team Tags summary Notes Web Chars year month publisher Editor
			# Penciller Inker Writer Cover Artist Colorist Letterer Language Genre Format Location genret

			self.ok = Button()
			self.cancel = Button()
			self.reset = Button()
			self.rebuild = Button()
			self.help = Button()

			self.list = ListBox()

			self.genret = TextBox()
			self.translateinto = Label()
			self.titleT = Label()
			self.titleT.Tag = "L"

			self.series = CheckBox()
			self.year = CheckBox()
			self.month = CheckBox()
			self.title = CheckBox()
			self.inker = CheckBox()
			self.tags = CheckBox()
			self.notes = CheckBox()
			self.cover = CheckBox()
			self.summary = CheckBox()
			self.publisher = CheckBox()
			self.penciller = CheckBox()
			self.writer = CheckBox()
			self.team = CheckBox()
			self.web = CheckBox()
			self.colorist = CheckBox()
			self.language = CheckBox()
			self.translate = Button()
			self.translatelist = ListBox()
			self.genre = CheckBox()
			self.letterer = CheckBox()
			self.location = CheckBox()
			self.editor = CheckBox()
			self.format = CheckBox()
			self.age = CheckBox()
			self.chars = CheckBox()
			self.location = CheckBox()

			#
			# list
			#
			self.list.Location = Point(10, 10)
			self.list.Name = "list"
			self.list.Size = Size(175, 300)
			self.list.TabIndex = 1
			self.list.Text = "Publications"
			self.list.MultiColumn = False
			self.list.SelectionMode = ListBox.SelectionMode.One
			self.list.HorizontalScrollbar = True
			self.list.DoubleClick += self.DoubleClickM
			#self.list.Sorted = True
			#
			# ok
			#
			self.ok.DialogResult = DialogResult.OK
			self.ok.Location = Point(405, 250)
			self.ok.Name = "ok"
			self.ok.Size = Size(75, 23)
			self.ok.TabIndex = 2
			self.ok.Text = "&Ok"
			self.ok.UseVisualStyleBackColor = True
			self.ok.Click += self.button_Click
			self.ok.BackColor = Color.LightGreen
			#
			# cancel
			#
			self.cancel.DialogResult = DialogResult.Cancel
			self.cancel.Location = Point(405, 275)
			self.cancel.Name = "cancel"
			self.cancel.Size = Size(75, 23)
			self.cancel.TabIndex = 3
			self.cancel.Text = "&Cancel"
			self.cancel.UseVisualStyleBackColor = True
			self.cancel.Click += self.button_Click
			self.cancel.BackColor = Color.Red
			#
			# reset
			#
			self.reset.Location = Point(405, 10)
			self.reset.Name = "reset"
			self.reset.Size = Size(75, 23)
			self.reset.TabIndex = 4
			self.reset.Tag = "Reset the selections"
			self.reset.Text = "&Reset"
			self.reset.UseVisualStyleBackColor = True
			self.reset.Click += self.button_Click
			self.reset.BackColor = Color.LightYellow
			#
			# rebuild
			#
			self.rebuild.Location = Point(405, 40)
			self.rebuild.Name = "rebuild"
			self.rebuild.Size = Size(75, 23)
			self.rebuild.TabIndex = 100
			self.rebuild.Tag = "Rebuild the List"
			self.rebuild.Text = "Re&build"
			self.rebuild.UseVisualStyleBackColor = True
			self.rebuild.Click += self.button_Click
			self.rebuild.BackColor = Color.LightBlue
			#
			# Help
			#
			self.help.Location = Point(405, 70)
			self.help.Name = "help"
			self.help.Size = Size(75, 23)
			self.help.TabIndex = 101
			self.help.Tag = "General Help"
			self.help.Text = "&Help"
			self.help.UseVisualStyleBackColor = True
			self.help.Click += self.button_Click
			self.help.BackColor = Color.Yellow
			#
			# Genre Text
			#
			self.genret.Location = Point(375, 225)
			self.genret.Name = "genret"
			self.genret.Size = Size(85, 25)
			self.genret.TabIndex = 27
			self.genret.Text = "Disney"
			#self.genret.TextAlign = MiddleCenter
			#
			# series
			#
			self.series.Location = Point(195, 25)
			self.series.Name = "series"
			self.series.Size = Size(103, 23)
			self.series.TabIndex = 5
			self.series.Tag = "Series"
			self.series.Text = "Series"
			self.series.UseVisualStyleBackColor = True
			self.series.CheckState = CheckState.Indeterminate
			self.series.ThreeState = True
			#
			# title
			#
			self.title.Location = Point(195, 50)
			self.title.Name = "title"
			self.title.Size = Size(20, 23)
			self.title.TabIndex = 6
			self.title.Tag = "Main Title"
			self.title.Text = ""
			self.title.UseVisualStyleBackColor = True
			self.title.CheckState = CheckState.Indeterminate
			self.title.ThreeState = True
			#
			# title label
			#
			self.titleT.Location = Point(213, 54)
			self.titleT.Name = "titleT"
			self.titleT.Size = Size(83, 23)
			self.titleT.TabIndex = 300
			self.titleT.Text = ">title<"
			self.titleT.Click += self.ClickT
			#
			# team
			#
			self.team.Location = Point(195, 75)
			self.team.Name = "team"
			self.team.Size = Size(103, 23)
			self.team.TabIndex = 7
			self.team.Tag = "Team"
			self.team.Text = "Team"
			self.team.UseVisualStyleBackColor = True
			self.team.CheckState = CheckState.Indeterminate
			self.team.ThreeState = True
			# Team Button
			#self.teamlist.Location = Point(405, 75)
			#self.teamlist.Name = "teamlist"
			#self.teamlist.Size = Size(75, 23)
			#self.teamlist.TabIndex = 28
			#self.teamlist.Tag = "TeamList"
			#self.teamlist.Text = "Team List"
			#self.teamlist.UseVisualStyleBackColor = True
			#self.teamlist.Click += self.button_Click
			#
			# Tags
			#
			self.tags.Location = Point(195, 100)
			self.tags.Name = "tags"
			self.tags.Size = Size(103, 23)
			self.tags.TabIndex = 8
			self.tags.Tag = "Tags"
			self.tags.Text = "Tags"
			self.tags.UseVisualStyleBackColor = True
			self.tags.CheckState = CheckState.Indeterminate
			self.tags.ThreeState = True
			#
			# summary
			#
			self.summary.Location = Point(195, 125)
			self.summary.Name = "summary"
			self.summary.Size = Size(103, 23)
			self.summary.TabIndex = 9
			self.summary.Tag = "Summary"
			self.summary.Text = "Summary"
			self.summary.UseVisualStyleBackColor = True
			self.summary.CheckState = CheckState.Indeterminate
			self.summary.ThreeState = True
			#
			# Notes
			#
			self.notes.Location = Point(195, 150)
			self.notes.Name = "notes"
			self.notes.Size = Size(103, 23)
			self.notes.TabIndex = 10
			self.notes.Tag = "Notes"
			self.notes.Text = "Notes"
			self.notes.UseVisualStyleBackColor = True
			self.notes.CheckState = CheckState.Indeterminate
			self.notes.ThreeState = True
			#
			# Web
			#
			self.web.Location = Point(195, 175)
			self.web.Name = "web"
			self.web.Size = Size(103, 23)
			self.web.TabIndex = 11
			self.web.Tag = "Web"
			self.web.Text = "Web"
			self.web.UseVisualStyleBackColor = True
			self.web.CheckState = CheckState.Indeterminate
			self.web.ThreeState = True
			#
			# Chars
			#
			self.chars.Location = Point(195, 200)
			self.chars.Name = "chars"
			self.chars.Size = Size(103, 23)
			self.chars.TabIndex = 12
			self.chars.Tag = "Characters"
			self.chars.Text = "Characters"
			self.chars.UseVisualStyleBackColor = True
			self.chars.CheckState = CheckState.Indeterminate
			self.chars.ThreeState = True
			#
			# year
			#
			self.year.Location = Point(195, 225)
			self.year.Name = "year"
			self.year.Size = Size(103, 23)
			self.year.TabIndex = 13
			self.year.Tag = "Year"
			self.year.Text = "Year"
			self.year.UseVisualStyleBackColor = True
			self.year.CheckState = CheckState.Indeterminate
			self.year.ThreeState = True
			#
			# month
			#
			self.month.Location = Point(195, 250)
			self.month.Name = "month"
			self.month.Size = Size(103, 23)
			self.month.TabIndex = 14
			self.month.Tag = "Month"
			self.month.Text = "Month"
			self.month.UseVisualStyleBackColor = True
			self.month.CheckState = CheckState.Indeterminate
			self.month.ThreeState = True
			#
			# publisher
			#
			self.publisher.Location = Point(195, 275)
			self.publisher.Name = "publisher"
			self.publisher.Size = Size(103, 23)
			self.publisher.TabIndex = 15
			self.publisher.Tag = "Publisher"
			self.publisher.Text = "Publisher"
			self.publisher.UseVisualStyleBackColor = True
			self.publisher.CheckState = CheckState.Indeterminate
			self.publisher.ThreeState = True
			#
			# Editor
			#
			self.editor.Location = Point(300, 25)
			self.editor.Name = "editor"
			self.editor.Size = Size(103, 23)
			self.editor.TabIndex = 16
			self.editor.Tag = "Editor"
			self.editor.Text = "Editor"
			self.editor.UseVisualStyleBackColor = True
			self.editor.CheckState = CheckState.Indeterminate
			self.editor.ThreeState = True
			#
			# Penciller
			#
			self.penciller.Location = Point(300, 50)
			self.penciller.Name = "penciller"
			self.penciller.Size = Size(103, 23)
			self.penciller.TabIndex = 17
			self.penciller.Tag = "Penciller"
			self.penciller.Text = "Penciller"
			self.penciller.UseVisualStyleBackColor = True
			self.penciller.CheckState = CheckState.Indeterminate
			self.penciller.ThreeState = True
			#
			# Inker
			#
			self.inker.Location = Point(300, 75)
			self.inker.Name = "inker"
			self.inker.Size = Size(103, 23)
			self.inker.TabIndex = 18
			self.inker.Tag = "Inker"
			self.inker.Text = "Inker"
			self.inker.UseVisualStyleBackColor = True
			self.inker.CheckState = CheckState.Indeterminate
			self.inker.ThreeState = True
			#
			# Writer
			#
			self.writer.Location = Point(300, 100)
			self.writer.Name = "writer"
			self.writer.Size = Size(103, 23)
			self.writer.TabIndex = 19
			self.writer.Tag = "Writer"
			self.writer.Text = "Writer"
			self.writer.UseVisualStyleBackColor = True
			self.writer.CheckState = CheckState.Indeterminate
			self.writer.ThreeState = True
			#
			# Cover Artist
			#
			self.cover.Location = Point(300, 125)
			self.cover.Name = "cover"
			self.cover.Size = Size(103, 23)
			self.cover.TabIndex = 20
			self.cover.Tag = "Cover Artist"
			self.cover.Text = "Cover Artist"
			self.cover.UseVisualStyleBackColor = True
			self.cover.CheckState = CheckState.Indeterminate
			self.cover.ThreeState = True
			#
			# Colorist
			#
			self.colorist.Location = Point(300, 150)
			self.colorist.Name = "colorist"
			self.colorist.Size = Size(103, 23)
			self.colorist.TabIndex = 21
			self.colorist.Tag = "Colorist"
			self.colorist.Text = "Colorist"
			self.colorist.UseVisualStyleBackColor = True
			self.colorist.CheckState = CheckState.Indeterminate
			self.colorist.ThreeState = True
			#
			# Letterer
			#
			self.letterer.Location = Point(300, 175)
			self.letterer.Name = "letterer"
			self.letterer.Size = Size(103, 23)
			self.letterer.TabIndex = 22
			self.letterer.Tag = "Letterer"
			self.letterer.Text = "Letterer"
			self.letterer.UseVisualStyleBackColor = True
			self.letterer.CheckState = CheckState.Indeterminate
			self.letterer.ThreeState = True
			#
			# Language
			#
			self.language.Location = Point(300, 200)
			self.language.Name = "language"
			self.language.Size = Size(103, 23)
			self.language.TabIndex = 23
			self.language.Tag = "Language"
			self.language.Text = "Language"
			self.language.UseVisualStyleBackColor = True
			self.language.CheckState = CheckState.Indeterminate
			self.language.ThreeState = True
			#
			# Translate Button
			self.translate.Location = Point(405, 100)
			self.translate.Name = "translate"
			self.translate.Size = Size(75, 23)
			self.translate.TabIndex = 28
			self.translate.Tag = "Translate"
			self.translate.Text = "Translate"
			self.translate.UseVisualStyleBackColor = True
			self.translate.Click += self.button_Click
			#
			# Translatelist
			#
			self.translatelist.Location = Point(405, 125)
			self.translatelist.Name = "translatelist"
			self.translatelist.Size = Size(75, 100)
			self.translatelist.TabIndex = 29
			self.translatelist.Text = "Translation Language"
			self.translatelist.MultiColumn = False
			self.translatelist.SelectionMode = ListBox.SelectionMode.One
			self.translatelist.HorizontalScrollbar = True
			self.translatelist.DoubleClick += self.DoubleClick
			#
			# Translateinto label
			#
			self.translateinto.Location = Point(405, 125)
			self.translateinto.Name = "translateinto"
			self.translateinto.Size = Size(75, 100)
			self.translateinto.TabIndex = 30
			self.translateinto.Text = ""
			#
			# Genre
			#
			self.genre.Location = Point(300, 225)
			self.genre.Name = "genre"
			self.genre.Size = Size(103, 23)
			self.genre.TabIndex = 24
			self.genre.Tag = "Genre"
			self.genre.Text = "Genre"
			self.genre.UseVisualStyleBackColor = True
			self.genre.CheckStateChanged += self.ChangeStatus
			self.genre.CheckState = CheckState.Indeterminate
			self.genre.ThreeState = True
			#
			# Format
			#
			self.format.Location = Point(300, 250)
			self.format.Name = "format"
			self.format.Size = Size(103, 23)
			self.format.TabIndex = 25
			self.format.Tag = "Format"
			self.format.Text = "Format"
			self.format.UseVisualStyleBackColor = True
			self.format.CheckState = CheckState.Indeterminate
			self.format.ThreeState = True
			#
			# Location
			#
			self.location.Location = Point(300, 275)
			self.location.Name = "location"
			self.location.Size = Size(103, 23)
			self.location.TabIndex = 26
			self.location.Tag = "Location"
			self.location.Text = "Location"
			self.location.UseVisualStyleBackColor = True
			self.location.CheckState = CheckState.Indeterminate
			self.location.ThreeState = True
			#
			# box W > x L V
			self.ClientSize = Size(490, 315)
			#
			self.Controls.Add(self.cancel)
			self.Controls.Add(self.ok)
			self.Controls.Add(self.reset)
			self.Controls.Add(self.rebuild)
			self.Controls.Add(self.help)

			self.Controls.Add(self.genret)

			self.Controls.Add(self.series)
			self.Controls.Add(self.publisher)
			self.Controls.Add(self.title)
			self.Controls.Add(self.titleT)
			self.Controls.Add(self.month)
			self.Controls.Add(self.year)
			self.Controls.Add(self.tags)
			self.Controls.Add(self.inker)
			self.Controls.Add(self.writer)
			self.Controls.Add(self.penciller)
			self.Controls.Add(self.team)
			self.Controls.Add(self.cover)
			self.Controls.Add(self.summary)
			self.Controls.Add(self.notes)

			self.Controls.Add(self.web)
			self.Controls.Add(self.colorist)
			self.Controls.Add(self.language)
			self.Controls.Add(self.translate)
			self.Controls.Add(self.genre)
			self.Controls.Add(self.letterer)
			self.Controls.Add(self.editor)
			self.Controls.Add(self.chars)
			self.Controls.Add(self.format)
			self.Controls.Add(self.location)

			self.list.BeginUpdate()
			#
			# remove first element
			#
			nIndex = 0
			aList.pop(0)

			for x in range(len(aList)):
				try:
					self.list.Items.Add(aList[x][0] + " - " + aList[x][3].decode('utf-8'))
				except:
					self.list.Items.Add(aList[x][0] + " - " + aList[x][3])

				try:
					if StartPub[0] == aList[x][3].decode('utf-8') and StartPub[1][:2] == LStart:
						nIndex = x
				except:
					if StartPub[0] == aList[x][3] and StartPub[1][:2] == LStart:
						nIndex = x

			self.Controls.Add(self.list)

			if nIndex > 0:
				self.list.SelectedIndex = nIndex
			else:
				self.list.SelectedIndex = self.list.FindString(StartPub[1])

			self.list.TopIndex = self.list.SelectedIndex

			self.list.Focus()

			#self.list.ScrollIntoView(1) #self.list.Items[self.list.SelectedIndex])

			self.list.EndUpdate()

			self.FormBorderStyle = FormBorderStyle.FixedDialog
			self.Name = "FromDucks"
			self.StartPosition = FormStartPosition.CenterParent
			self.Text = "(c) Inducks team (web: inducks.org)"
			self.MinimizeBox = False
			self.MaximizeBox = False


		def button_Click(self, sender, e):
			from System.IO import FileInfo
			from System.Windows.Forms import (
				MessageBox, CheckState
			)

			global aUpdate, SelInd

			if sender.Name.CompareTo(self.ok.Name) == 0:
				SelInd = self.list.SelectedIndex
				dDict = {"Checked": 1, "Unchecked": 0, "Indeterminate": 2 }
				for x in range(self.Controls.Count):
					if 4 < self.Controls.Item[x].TabIndex < 27:
						aUpdate[self.Controls.Item[x].TabIndex- 5] = dDict[self.Controls.Item[x].CheckState.ToString()]
				aUpdate[22] = self.genret.Text

			elif sender.Name.CompareTo(self.reset.Name) == 0:
				for x in range(self.Controls.Count):
					if 4 < self.Controls.Item[x].TabIndex < 27:
						aUpdate[self.Controls.Item[x].TabIndex-5] = aUpdate[self.Controls.Item[x].TabIndex-5] + 1
						if aUpdate[self.Controls.Item[x].TabIndex-5] == 3:
							aUpdate[self.Controls.Item[x].TabIndex-5] = 0
						if self.Controls.Item[x].CheckState == CheckState.Checked:
							self.Controls.Item[x].CheckState = CheckState.Unchecked
						elif self.Controls.Item[x].CheckState == CheckState.Unchecked:
							self.Controls.Item[x].CheckState = CheckState.Indeterminate
						else:
							self.Controls.Item[x].CheckState = CheckState.Checked

				self.translateinto.Visible = False
				self.translateinto.Text = ""
				TranslationID = ""

			elif sender.Name.CompareTo(self.rebuild.Name) == 0:
				FillDat(True, None)

			elif sender.Name.CompareTo(self.cancel.Name) == 0:
				pass

			elif sender.Name.CompareTo(self.translate.Name) == 0:
				self.translatelist.BeginUpdate()
				if self.translatelist.Visible == False:

					self.translatelist.Visible = True
					self.translateinto.Text = ""
					self.translateinto.Visible = False

				if FileInfo(__file__[:-len('FromDucks.py')] + 'Languages.dat').Exists:
					fileHandle = open(__file__[:-len('FromDucks.py')] + 'Languages.dat', 'r')
					AllLanguages = fileHandle.readlines()
					fileHandle.close()
					AllLanguages.pop(0)
					for x in AllLanguages:
						cList = x.split("^")
						self.translatelist.Items.Add(cList[0].upper() + " - " + cList[2] )
					self.Controls.Add(self.translatelist)
					self.translatelist.EndUpdate()
				else:
					MessageBox.Show('REBUILD the local tables!')

			elif sender.Name.CompareTo(self.help.Name) == 0:
				MessageBox.Show('Help - FromDucks Script v' + VERSION + "\n---------------------\n" +
				"Select 1 or more comics, same series;\nStart the script and define the desired behavior.\n" +
				"Pick the correct Series name for COA, select which fields to fill:\n" +
				"* A marked box means OVERRIDE ALWAYS *\n" +
				"* A shaded box means OVERRIDE IF EMPTY *\n" +
				"(only for SUMMARY, it will add to the current value)\n" +
				"* A blank box means DO NOT OVERRIDE *\n" +
				"On the Title label, clicking will change the way the title is written, from original to all initials capitalized\n" +
				"Choose the genre if needed and a translation language with a double click\n"+
				"(characters' names will be set in that language, if existing)\n" +
				"\nDouble clicking on a series will show the COA Webpage;\n" +
				"Click OK and wait...\n" +
				"> RESET will cycle the checkbox status\n" +
				"> REBUILD will rebuild the Series list\n(the Series list is read once and then stored);\n" +
				"> CANCEL will abort the script")
		def ChangeStatus(self, sender, e):

			if sender.Name.CompareTo(self.genre.Name) == 0:
				stDict = {"Checked": True, "Unchecked": False, "Indeterminate": True }
				self.genret.Enabled = stDict[sender.CheckState.ToString()]
			else:
				pass

		def DoubleClickM(self, sender, e):
			from System.Diagnostics import Process
			global aList

			Code = aList[self.list.SelectedIndex][0]
			cWeb = "https://inducks.org/publication.php?c="
			DCWeb = cWeb + Code
			if DEBUG:log_BD("Series in Web:", DCWeb)
			Process.Start(DCWeb)

		def DoubleClick(self, sender, e):
			global TranslationID

			if self.translateinto.Visible == False:
				self.translateinto.Visible = True
			TranslationID = self.translatelist.SelectedItem
			self.translatelist.Visible = False
			self.Controls.Add(self.translateinto)
			self.translateinto.Text = self.translatelist.SelectedItem[self.translatelist.SelectedItem.find(" ")+2:].strip(" ")

		def ClickT(self, sender, e):
			global TitleT

			if self.titleT.Tag == "T":
				self.titleT.Text = ">title<"
				self.titleT.Tag = "L"
				TitleT = "L"
			else:
				self.titleT.Text = ">TiTlE<"
				self.titleT.Tag = "T"
				TitleT	= "T"
	
	def WorkerThread(books):
		from System.IO import FileInfo
		from System.Windows.Forms import (
			MessageBox, Application
		)

		global aUpdate, TranslationID, f, TitleT #, progress

		try:

			dTeams = ("TA", "BB", "TLP", "JW","SD", "BBB", "TTC","TMU", "Evrons", "CDR","Tempolizia", "UH", "Foul Fellows' Club", "101","S7", "QW", "SCPD", "Evil dwarfs", "DWM", "Justice Ducks")

			#  Read Language for Characternames
			Translations = ""
			if TranslationID == "":
				TranslationID = 'en'
			else:
				TranslationID = TranslationID[:2]

			#  Read Characternames
			if FileInfo(__file__[:-len('FromDucks.py')] + 'Characters.dat').Exists:
				fileHandle = open(__file__[:-len('FromDucks.py')] + 'Characters.dat', 'r')
				Translations = fileHandle.readlines()
				fileHandle.close()
			else:
				MessageBox.Show("Translations of characters' name not available. Please REBUILD the tables!")

			#progress = Stats()
			#progress.CenterToParent()
			#progress.Show()

			nBook = 1
			lErrors = True
			log_BD("\n ** Scraping Started ** " + str(books.Count) + " Album(s)", "\n============ " + str(datetime.now().strftime("%A %d %B %Y %H:%M:%S")) + " ===========", 0)

			f = ProgressBarDialog(books.Count)
			f.Show(ComicRack.MainWindow)

			for book in books:
				Application.DoEvents()

				#progress.modifyStats(nBook, TotBooks, book.Series, str(book.Number))

				f.Update("[" + str(nBook) + "/" + str(len(books)) + "] : " + "[" + book.Series +"] #" + book.Number + " - " + book.Title, 1)
				f.Refresh()

				nBook += 1

				Series,Day,Month,Year,Publisher,Pages,Language,Web,StoryFull,Characters,MainTitle = ReadInfoDucks(book)
				#
				if DEBUG:print(chr(10) , "Scraped Info: " , Series,Day,Month,Year,Publisher,Pages,Language,Web,StoryFull,Characters,MainTitle)
				#
				# series = 0 title = 1 team = 2 tags = 3 summary = 4 notes = 5 web = 6 chars = 7 year = 8 month = 9 publisher = 10
				# editor = 11 penciller = 12  inker = 13  writer = 14 cover = 15 colorist = 16 letterer = 17 language = 18
				# genre = 19 format = 20 location = 21 genret = 22

				# if Series == 0:

					# progress.Hide()
					# return

				if Series != -1:

					if (aUpdate[0] == 1) or (aUpdate[0] == 2 and book.Series == ""):
						try:
							book.Series = Series.decode('utf-8')
						except:
							book.Series = Series

					if (aUpdate[1] == 1) or (aUpdate[1] == 2 and book.Title == ""):
						if MainTitle == "":
							for x in StoryFull:
								if TypeCol(x[0]) == "Story":
									if TitleT == "T":
										book.Title = x[2].title().strip("(").strip(")").replace("'S","'s").replace(" The"," the").replace("&Amp;","&")      #.decode('utf-8')
									else:
										book.Title = x[2].strip("(").strip(")").replace("'S","'s").replace(" The"," the").replace("&Amp;","&")      #.decode('utf-8')
									break
						else:
							if TitleT == "T":
								book.Title = MainTitle.title().strip("(").strip(")").replace("'S","'s").replace(" The "," the ").replace("&Amp;","&")
							else:
								book.Title = MainTitle.strip("(").strip(")").replace("'S","'s").replace(" The "," the ").replace("&Amp;","&")

					if (aUpdate[4] == 1):
						book.Summary = SumBuild(StoryFull)
					if (aUpdate[4] == 2):
						book.Summary = book.Summary + chr(10) * 2 + SumBuild(StoryFull)

					if (aUpdate[5] == 1) or (aUpdate[5] == 2 and book.Notes == ""):
						book.Notes = "Scraped from I.N.D.U.C.K.S. " + str(datetime.now())

					if (aUpdate[6] == 1) or (aUpdate[6] == 2 and book.Web == ""):
						book.Web = Web

					if (aUpdate[7] == 1) or (aUpdate[7] == 2 and book.Characters == ""):
						Charlist = ""
						cTeams = ""
						for x in Characters:
							Chars = ""
							CharName = x[0].replace("+","")
							m0 = re.compile(CharName + '\\^' + TranslationID + '\\^(.*?)\\^Y', re.IGNORECASE)
							for Person in Translations:
								result0 = m0.search(Person)
								if result0:
									try:
										Chars = result0.group(1).decode('utf-8')
										break
									except:
										pass

							if Chars == "":
								try:
									Chars = x[1].decode('utf-8')
								except:
									Chars = x[1]

							if Chars not in Charlist:
								Charlist += Chars + ", "
								if CharName in dTeams:
									cTeams += Chars +", "

						book.Characters = Charlist.strip(", ")

						if ((aUpdate[2] == 1) or (aUpdate[2] == 2 and book.Teams == "")) and cTeams != "":
							book.Teams = cTeams.strip(", ")

					if (aUpdate[8] == 1) or (aUpdate[8] == 2 and book.Year == ""):
						if Year == 0:
							book.Year = -1
						else:
							book.Year = int(Year)

					if (aUpdate[9] == 1) or (aUpdate[9] == 2 and book.Month == ""):
						if Month == 0:
							book.Month = -1
						else:
							book.Month = int(Month)
						if Day == 0:
							book.Day = -1
						else:
							try:
								book.Day = int(Day)
							except:
								continue

					if (aUpdate[10] == 1) or (aUpdate[10] == 2 and book.Publisher == ""):
						try:
							book.Publisher = Publisher.decode('utf-8')
						except:
							book.Publisher = Publisher

					Writer,Penciller,Inker,CoverArtist,Letterer,Colorist = ArtBuild(StoryFull)

					if (aUpdate[12] == 1) or (aUpdate[12] == 2 and book.Penciller == ""):
						book.Penciller = Penciller

					if (aUpdate[13] == 1) or (aUpdate[13] == 2 and book.Inker == ""):
						book.Inker = Inker

					if (aUpdate[14] == 1) or (aUpdate[14] == 2 and book.Writer == ""):
						book.Writer = Writer

					if (aUpdate[15] == 1) or (aUpdate[15] == 2 and book.CoverArtist == ""):
						book.CoverArtist = CoverArtist

					if (aUpdate[16] == 1) or (aUpdate[16] == 2 and book.Colorist == ""):
						book.Colorist = Colorist

					if (aUpdate[17] == 1) or (aUpdate[17] == 2 and book.Letterer == ""):
						book.Letterer = Letterer

					if (aUpdate[18] == 1) or (aUpdate[18] == 2 and book.LanguageISO == ""):
						book.LanguageISO = Language

					if (aUpdate[19] == 1) or (aUpdate[19] == 2 and book.Genre == ""):
						try:
							book.Genre = aUpdate[22].decode('utf-8')
						except:
							book.Genre = aUpdate[22]

				elif Series == -1:
					lErrors = False
					debuglog()

				log_BD("[" + book.Series +"] #" + book.Number + " - " + book.Title," ** Scraped **\n", 1 )
				Application.DoEvents()

		except:
			f.Close()
			debuglog()
			return False

		#progress.Hide()
		f.Update("Completed !", 1)
		f.Refresh()
		f.Close()

		return lErrors

	def FillDat(lForce,configpath):
		from System.IO import FileInfo
		from System.Windows.Forms import (
			MessageBox
		)

		if not configpath:
			configpath= []
			configpath.append(__file__[:-len('FromDucks.py')] + "FromDucks.dat")
			configpath.append(__file__[:-len('FromDucks.py')] + "Characters.dat")
			configpath.append(__file__[:-len('FromDucks.py')] + "Languages.dat")

		if lForce:
			fd = ProgressBarDialog(3, "Rebuilding")
			fd.Show(ComicRack.MainWindow)
			#progressDlg = Progress()
			#progressDlg.CenterToParent()
			#progressDlg.Show()
			fd.Update("Reading/Rebuilding [DB]", 1, "Local Database ")
			fd.Refresh()

		for cycle in configpath:

			if "FromDucks.dat" in cycle:
				cWeb='https://inducks.org/inducks/isv/inducks_publication.isv'
			elif "Characters.dat" in cycle:
				cWeb='https://inducks.org/inducks/isv/inducks_charactername.isv'
			elif "Languages.dat" in cycle:
				cWeb='https://inducks.org/inducks/isv/inducks_language.isv'

			if lForce or not FileInfo(cycle).Exists:
				try:
					fileHandle = _read_url(cWeb)
					if lForce:
						fd.Update("Reading/Rebuilding [" + cWeb + "]", 1, "Local Database ")
						fd.Refresh()
						#progressDlg.modifyPrompt("Reading/Rebuilding [" + cWeb + "]")

					#contents = fileHandle.readlines()

				except IOError:
					MessageBox.Show('Cannot open URL ' + cWeb + 'for reading')
					sys.exit(0)

				#fileHandle.close()

				#fileHandle = open(cycle, 'w')

				open(cycle, 'wb').write(fileHandle.encode("utf-8"))

				#for line in contents:
				#	fileHandle.write(line)

				#fileHandle.close()

			elif not FileInfo(cycle).Exists:

				MessageBox.Show('Cannot open ' + cycle + '\nPlease REBUILD it!')
				sys.exit(0)

		if lForce:
			fd.Close()
			#progressDlg.Close()
			MessageBox.Show('REBUILD completed!')


	from System.Diagnostics import Process
	from System.Threading import Monitor
	from System.IO import Path, FileInfo, File
	from System.Windows.Forms import (
		MessageBox, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton,
		DialogResult
	)

	import clr;
	clr.AddReference('System')
	clr.AddReference('System.Windows.Forms')
	clr.AddReference('System.Drawing')

	global settings, aList, StartPub, SelInd, TranslationID, LStart, DEBUG, TitleT

	configpath = []
	TranslationID = ""
	StartPub = []
	LStart = ""

	TitleT = "L"

	bdlogfile = Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Rename_Log.txt")
	if FileInfo(bdlogfile).Exists and FileInfo(bdlogfile).Length > SIZE_RENAME_LOG:
		Result = MessageBox.Show(ComicRack.MainWindow, "The File FromDucks_Rename_Log.txt is growing too much: do you want to clean it ?", "Rename Log File", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
		if Result == DialogResult.Yes:
			File.Delete(bdlogfile)

	debuglogfile = Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Debug_Log.txt")
	if FileInfo(debuglogfile).Exists and FileInfo(debuglogfile).Length > SIZE_DEBUG_LOG:
		Result = MessageBox.Show(ComicRack.MainWindow, "The File FromDucks_Debug_Log.txt is growing too much: do you want to clean it ?", "Debug Log File", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
		if Result == DialogResult.Yes:
			File.Delete(debuglogfile)

	Spath = sys.path[:]
	del Spath[0]

	#Just in case no paths were found
	if not Spath:
		MessageBox.Show("The script cannot find the locations of the scripts. Please try restarting ComicRack.")
		return

	configpath.append(__file__[:-len('FromDucks.py')] + "FromDucks.dat")

	if not FileInfo(configpath[0]).Exists:
		fileHandle = open(__file__[:-len('FromDucks.py')] + "FromDucks.dat", 'w')
		fileHandle.write(" ")
		fileHandle.close()

	if FileInfo(configpath[0]).Length <= 1:
		FillDat(True,None)
	else:
		FillDat(False,configpath)

	for book in books:
		StartPub.append(book.Series)
		if book.LanguageISO == "en":
			LStart = "us"
		else:
			LStart = book.LanguageISO
		StartPub.append(LStart + "/" + book.Series[:1])
		break

	fileHandle = open(configpath[0], 'r')
	pubDB = fileHandle.readlines()
	fileHandle.close()

	aList=list()

	for x in pubDB:
		if not x[1:] == "":
			Data=x.split("^")
			aList.append([Data[0],Data[1],Data[2],Data[3]])

	import operator
	aList.sort(key=operator.itemgetter(0,1))

	bf = BuilderForm()

	if bf.ShowDialog() == DialogResult.OK:
		if (books):

			if WorkerThread(books):
				Monitor.Enter(ComicRack.MainWindow)
				log_BD("\nAlbum(s) scraped !", "", 0)
				log_BD("============= " + str(datetime.now().strftime("%A %d %B %Y %H:%M:%S")) + " =============", "\n\n", 0)
				rdlg = MessageBox.Show(ComicRack.MainWindow, "Scrape Completed ! \n\nOpen Rename Log ?", "FromDucks", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
				if rdlg == DialogResult.Yes:
					# open rename log automatically
					if FileInfo(Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Rename_Log.txt")).Exists:
						Process.Start (Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Rename_Log.txt"))

			else:
				Monitor.Enter(ComicRack.MainWindow)
				log_BD("\n!! Errors in scraping !!", "", 0)
				log_BD("============= " + str(datetime.now().strftime("%A %d %B %Y %H:%M:%S")) + " =============", "\n\n", 0)
				rdlg = MessageBox.Show(ComicRack.MainWindow, "Scrape ended with errors ! \n\nOpen Debug Log ?", "FromDucks", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
				if rdlg == DialogResult.Yes:
					# open debug log automatically
					if FileInfo(Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Debug_Log.txt")).Exists:
						Process.Start (Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Debug_Log.txt"))

		Monitor.Exit(ComicRack.MainWindow)


def ReadInfoDucks(book):

	global aList, SelInd, TranslationID, f #, progress

	cSeries = aList[SelInd][0].strip()


	cWeb = 'https://inducks.org/issue.php?c='
	if DEBUG:log_BD("Searching ---->" , aList[SelInd][3])
	
	nNumIss = str(book.Number).strip().replace(" ","%20").replace("/","%2F")	
	contents = ""
	for counter in range (0,4):
		try:
			nNum = cSeries + "+" * counter + nNumIss
			#nNum = cSeries + "%20" + nNumIss

			try:
				pr = cWeb + nNum

				#req = urllib2.Request.(pr)
				contents = _read_url(pr.encode('utf-8'))

			except:
				debuglog()
				continue

				#if not "Issue not found" in contents:
			if len(contents)>4000:
				break

		except IOError:
			f.Update("Not Found: N." + str(nNum), 1)
			f.Refresh()
			#
			if DEBUG:log_BD("Not Found ----> n. " , nNum.replace("+",""))
			debuglog()
			#
			return -1,0,0,0,0,0,0,0,0,0,0
def ReadInfoDucks(book):
	"""
	Refactored: Accepts a dict with keys 'Series', 'Number', 'LanguageISO'.
	Returns a dict with fields: series, day, month, year, publisher, pages, language, web, stories, characters, main_title.
	"""
	import re
	# Compatibility imports for Python 2 and 3
	try:
		import urllib2 as urllib_request
	except ImportError:
		import urllib.request as urllib_request

	# Extract input
	cSeries = book.get('Series', '').strip()
	nNumIss = str(book.get('Number', '')).strip().replace(' ', '%20').replace('/', '%2F')
	language = book.get('LanguageISO', 'en')
	cWeb = 'https://inducks.org/issue.php?c='
	contents = ''
	web_url = ''
	# Try up to 4 times with +
	for counter in range(0, 4):
		nNum = cSeries + ('+' * counter) + nNumIss
		pr = cWeb + nNum
		web_url = pr
		try:
			with urllib_request.urlopen(pr) as resp:
				contents = resp.read().decode('utf-8')
			if len(contents) > 4000:
				break
		except Exception:
			continue
	if len(contents) < 4000:
		return {}
	# Parse fields
	m0 = re.compile(r'Publication<.*?<a\shref="publication\.php\?c=[a-z]{2,3}[%2][^">]*?">(.*?)<', re.IGNORECASE | re.MULTILINE | re.DOTALL)
	m1 = re.compile(r'Publisher.*?<a\shref="publisher.php\?c=.*?">(.*?)<', re.IGNORECASE | re.MULTILINE | re.DOTALL)
	m1a = re.compile(r'Title<\/dt>\s*?<dd>(.*?)<', re.IGNORECASE | re.MULTILINE | re.DOTALL)
	m2 = re.compile(r'Date<\/dt>\s*?<dd>(.*?)>', re.IGNORECASE | re.MULTILINE | re.DOTALL)
	m3 = re.compile(r'Pages<\/dt>\s*?<dd>(\d*?)<', re.IGNORECASE | re.MULTILINE | re.DOTALL)
	result0 = m0.search(contents)
	series = result0.group(1) if result0 else ''
	result1 = m1.search(contents)
	publisher = result1.group(1) if result1 else ''
	result1a = m1a.search(contents)
	main_title = result1a.group(1) if result1a else ''
	result2 = m2.search(contents)
	day = 0
	month = 0
	year = 0
	if result2:
		if '<time datetime=' in result2.group(1):
			day = result2.group(1)[24:26]
			month = result2.group(1)[21:23]
			year = result2.group(1)[16:20]
		else:
			d1 = re.compile(r'([\D]{0,10})\s')
			d2 = re.compile(r'([0-9]{4})')
			resultd1 = d1.search(result2.group(1))
			resultd2 = d2.search(result2.group(1))
			if resultd1:
				month = resultd1.group(1)
			if resultd2:
				year = resultd2.group(1)
	result3 = m3.search(contents)
	pages = result3.group(1) if result3 else 0
	# Stories and characters
	m_story = re.compile(r'<tr class="[^"]+" id=".+?</tr>', re.S)
	m_story_info = re.compile(r'id="(?P<type>[^"]+?)">.*?<a\shref="story\.php\?c=(?P<storycode>[^"]+)">(?:<i>\((?P<title>.*?)\)</i>)?', re.S)
	m_artists = re.compile(r'<dt>(?P<job>[^<]+).*?<a\shref="creator\.php\?c=[^"]+">(?P<artist_name>[^<]+)</a>')
	m_appearances = re.compile(r'<a\shref="character\.php\?c=(?P<character_code>[^"]+)">(?P<character_name>[^<]+)</a>')
	stories = []
	characters = []
	story_blocks = m_story.findall(contents)
	for story_html in story_blocks:
		type_ = ''
		story_id = ''
		title = ''
		artists = []
		result_info = m_story_info.search(story_html)
		if result_info:
			type_ = result_info.group('type')
			story_id = result_info.group('storycode')
			title = result_info.group('title')
		result_artists = m_artists.search(story_html)
		if result_artists:
			job = result_artists.group('job')
			name = result_artists.group('artist_name')
			artists.append(f'{job}^{name}')
		result_appearances = m_appearances.findall(story_html)
		for character_code, char_name in result_appearances:
			characters.append([character_code, char_name])
		stories.append({'type': type_, 'story': story_id, 'title': title, 'artists': artists})
		if len(stories) > 50:
			break
	return {
		'series': series,
		'day': day,
		'month': month,
		'year': year,
		'publisher': publisher,
		'pages': pages,
		'language': language,
		'web': web_url,
		'stories': stories,
		'characters': characters,
		'main_title': main_title
	}
	return cNotes

def ArtBuild(StoryFull):

	cInk =""
	cWri = ""
	cPen = ""
	cLett = ""
	cColour = ""
	cCover = ""

	for story in StoryFull:
		for Art in range (len(story[3])):
			Job, Auth = (story[3][Art]).split("^")
			if TypeCol(story[0]) == "Cover" and Auth not in cCover:
				cCover += Auth + ", "
			if Job in ("Writing", "Plot", "Script", "Text", "Idea") and Auth not in cWri:
				cWri += Auth    + ", "
			elif Job in ("Art", "Pencils") and Auth not in cPen:
				cPen += Auth + ", "
			elif Job == "Ink" and Auth not in cInk:
				cInk += Auth   + ", "
			elif Job == "Lettering" and Auth not in cLett:
				cLett += Auth  + ", "
			elif Job == "Colours" and Auth not in cColour:
				cColour +=  Auth  + ", "

	return cWri.strip(", "),cPen.strip(", " ),cInk.strip(", "),cCover.strip(", "),cLett.strip(", "),cColour.strip(", ")

def TypeCol(Type):

	if Type == 'cbdced':
		cType="Story"
	elif Type == 'ffcc33':
		cType="Cover"
	elif Type == 'cccc99':
		cType="Article"
	elif Type == 'ff99ff':
		cType="Gag"
	else:
		cType="Other"

	return cType

def MonthNum(month):

	dMonth={"Jan": 1, "Feb": 2, "Mar": 3 ,"Apr": 4,"May": 5,"Jun": 6,"Jul": 7,"Aug": 8,"Sep": 9,"Oct": 10,"Nov": 11,"Dec": 12}
	if month[:3] in dMonth:
		return dMonth[month[:3]]
	else:
		return 0

def debuglog():
	from System.IO import Path
	traceback = sys.exc_info()[2]
	stackTrace = []

	logfile = Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Debug_Log.txt")

	if DEBUG:log_BD("Writing Log to " + logfile)
	log = open(logfile, 'a')
	try:
		log.write (str(datetime.now().strftime("%A %d %B %Y %H:%M:%S")) + '\n')
		log.write ("".join(['Caught ', sys.exc_info()[0].__name__, ': ',sstr(sys.exc_info()[1]), '\n']))
		log_BD('Caught ', sys.exc_info()[0].__name__, ': ',sstr(sys.exc_info()[1]))
	except:
		pass
	while traceback is not None:
		frame = traceback.tb_frame
		lineno = traceback.tb_lineno
		code = frame.f_code
		filename = code.co_filename
		name = code.co_name
		stackTrace.append((filename, lineno, name))
		traceback = traceback.tb_next

	nL = 0
	for line in stackTrace:
		nL += 1
		log_BD(nL,"-",line)
		log.write (",".join("%s" % tup for tup in line))

	log.write ('\n')
	log.flush()
	log.close()

def sstr(object):
	''' safely converts the given object into a string (sstr = safestr) '''
	if object is None:
		return '<None>'
	if is_string(object):
		# this is needed, because str() breaks on some strings that have unicode
		# characters, due to a python bug.  (all strings in python are unicode.)
		return object
	return str(object)

def log_BD(bdstr,bdstat="",lTime=1):
	from System.IO import Path
	bdlogfile = Path.Combine(__file__[:-len('FromDucks.py')] , "FromDucks_Rename_Log.txt")

	bdlog = open(bdlogfile, 'a')
	if lTime == 1:
		cDT = str(datetime.now().strftime("%A %d %B %Y %H:%M:%S")) + " > "

	else:
		cDT= ""

	bdlog.write (str(cDT).encode("utf-8") + str(bdstr).encode("utf-8") + "   " + str(bdstat).encode("utf-8") + "\n")
	bdlog.flush()
	bdlog.close()

def is_string(object):
	''' returns a boolean indicating whether the given object is a string '''
	if object is None:
		return False
	return isinstance(object, str)

def _read_url(url):
	from System.Net import HttpWebRequest, DecompressionMethods
	from System.Text import Encoding
	from System.IO import StreamReader
	
	page = ''

	requestUri = url
	log_BD(requestUri, "Requesting", 1)
	try:
		Req = HttpWebRequest.Create(requestUri)
		Req.Timeout = 60000
		Req.UserAgent = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36'
		Req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip

		#Req.Referer = requestUri
		Req.Accept = 'text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8'
		Req.Headers.Add('Accept-Language','en-US,en;q=0.9,it;q=0.8,fr;q=0.7,de-DE;q=0.6,de;q=0.5')

		Req.KeepAlive = True
		webresponse = Req.GetResponse()
		a = webresponse.Cookies

		inStream = webresponse.GetResponseStream()
		encode = Encoding.GetEncoding("utf-8")
		ReadStream = StreamReader(inStream, encode)
		page = ReadStream.ReadToEnd()

	except:
		debuglog()

	try:
		inStream.Close()
		webresponse.Close()
	except:
		pass

	return page

def test_ReadInfoDucks():
	"""
	Test ReadInfoDucks with a sample comic dict and print the result.
	"""
	sample_book = {
		'Series': 'dk/AA',  # Example: Italian Disney series code
		'Number': '1958-26',        # Example issue number
		'LanguageISO': 'dk'   # Example language code
	}
	result = ReadInfoDucks(sample_book)
	print('Test ReadInfoDucks result:')
	for k, v in result.items():
		print(f'{k}: {v}')