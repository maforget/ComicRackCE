# SearchAndReplace.py
#
# Shows a dialog to replace values in Books
#
# You are free to modify and distribute this file
# (C)'2010 cYo Soft
##########################################################################

import clr
clr.AddReferenceByPartialName("System.Windows.Forms")
clr.AddReferenceByPartialName("System.Drawing")
from System.Windows.Forms import *
from System.Drawing import *

lastSelection = 0
lastSearchText = ""
lastReplaceText = ""

#@Name	Search and Replace...
#@Image	SearchAndReplace.png
#@Hook	Books
#@Description A script to to search and replace values in Books
def SearchAndReplace (books):

	global lastSelection, lastSearchText, lastReplaceText

	if books.Length == 0:
		return

	f = Form()
	
	# labelField
	labelField = Label()
	labelField.Location = Point(19, 22)
	labelField.AutoSize = True
	labelField.Text = ComicRack.Localize("SearchAndReplace", "labelField", "Field:")
	labelField.TabIndex = 0

	# cbFields
	cbFields = ComboBox()
	cbFields.DropDownStyle = ComboBoxStyle.DropDownList
	cbFields.Location = Point(77, 19)
	cbFields.Size = Size(262, 21)
	cbFields.Sorted = True
	cbFields.TabIndex = 1

	# labelSearch
	labelSearch = Label()
	labelSearch.Location = Point(19, 49)
	labelSearch.AutoSize = True
	labelSearch.Text = ComicRack.Localize("SearchAndReplace", "labelSearch", "Search:")
	labelSearch.TabIndex = 2

	# txtSearch
	txtSearch = TextBox()
	txtSearch.Location = Point(77, 46)
	txtSearch.Name = "txtSearch"
	txtSearch.Size = Size(262, 20)
	txtSearch.TabIndex = 3

	# labelReplace
	labelReplace = Label()
	labelReplace.Location = Point(19, 75)
	labelReplace.AutoSize = True
	labelReplace.Text = ComicRack.Localize("SearchAndReplace", "labelReplace", "Replace:")
	labelReplace.TabIndex = 4

	# txtReplace
	txtReplace = TextBox()
	txtReplace.Location = Point(77, 72)
	txtReplace.Size = Size(262, 20)
	txtReplace.TabIndex = 5

	# btOK
	btOK = Button()
	btOK.DialogResult = DialogResult.OK
	btOK.Location = Point(173, 109)
	btOK.Size = Size(80, 24)
	btOK.Text = ComicRack.Localize("SearchAndReplace", "btOK", "&OK")
	btOK.TabIndex = 6

	# btCancel
	btCancel = Button()
	btCancel.DialogResult = DialogResult.Cancel
	btCancel.Location = Point(259, 109)
	btCancel.Size = Size(80, 24)
	btCancel.Text = ComicRack.Localize("SearchAndReplace", "btCancel", "&Cancel")
	btCancel.TabIndex = 7

	# SearchAndReplace
	f.AcceptButton = btOK
	f.AutoScaleMode = AutoScaleMode.Font
	f.CancelButton = btCancel
	f.ClientSize = Size(346, 140)
	f.Controls.Add(btCancel)
	f.Controls.Add(btOK)
	f.Controls.Add(txtReplace)
	f.Controls.Add(labelReplace)
	f.Controls.Add(txtSearch)
	f.Controls.Add(labelSearch)
	f.Controls.Add(cbFields)
	f.Controls.Add(labelField)
	f.FormBorderStyle = FormBorderStyle.FixedDialog
	f.MaximizeBox = False
	f.MinimizeBox = False
	f.ShowIcon = False
	f.ShowInTaskbar = False
	f.StartPosition = FormStartPosition.CenterParent
	f.Text = ComicRack.Localize("SearchAndReplace", "Caption", "Search and Replace...")

	# this returns a dictionary of the type [display name, field]
	fields = ComicRack.App.GetComicFields()
	for k in fields.Keys:
		cbFields.Items.Add(k)

	cbFields.SelectedIndex = lastSelection
	txtReplace.Text = lastReplaceText
	txtSearch.Text = lastSearchText
	ThemeMe(f)


	#Show Window
	try:
		if f.ShowDialog(ComicRack.MainWindow)==DialogResult.OK:

			field = fields[cbFields.SelectedItem]
			search = txtSearch.Text
			replace = txtReplace.Text

			for book in books:
				value = getattr(book, field)
				value = value.Replace(search, replace)
				setattr(book, field, value)

		lastSelection = cbFields.SelectedIndex
		lastReplaceText = txtReplace.Text
		lastSearchText = txtSearch.Text
	finally:
		f.Dispose()

def ThemeMe(control):
    if ComicRack.App.ProductVersion >= '0.9.182':
            ComicRack.Theme.ApplyTheme(control)
