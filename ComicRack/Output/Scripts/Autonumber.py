# Autonumber.py
#
# Shows a dialog to number the passed books (series or alternate series)
#
# You are free to modify and distribute this file
# (C)'2007 cYo Soft
##########################################################################

import clr
clr.AddReferenceByPartialName("System.Windows.Forms")
clr.AddReferenceByPartialName("System.Drawing")
from System.Windows.Forms import *
from System.Drawing import *

def GetStart(start):

	try:
		result = float(start)
	except:
		result = 1
	return result

#@Name	Autonumber Wizard...
#@Image	Renumber.png
#@Key	Autonumber
#@Hook	Books
#@Description A script to renumber the selected Books
def RenumberBooks(books):

	if books.Length == 0:
		return

	f = Form()
	f.SuspendLayout()

	#Label
	label = Label()
	label.AutoSize = True
	label.Location = Point(12, 53)
	label.TabIndex = 0
	label.Text = ComicRack.Localize("Autonumber", "labelBegin", "Begin at Number:")
	f.Controls.Add (label)
	
	# First numeric up down
	nudStart = NumericUpDown()
	nudStart.Location = Point(166, 51)
	nudStart.Size = Size(65, 20)
	nudStart.TabIndex = 1
	nudStart.TextAlign = HorizontalAlignment.Right
	nudStart.Maximum = 100000000
	f.Controls.Add (nudStart)

	# Second numeric up down
	nudEnd = NumericUpDown()
	nudEnd.Location = Point(166, 76)
	nudEnd.Size = Size(65, 20)
	nudEnd.TabIndex = 3
	nudEnd.TextAlign = HorizontalAlignment.Right
	nudEnd.Maximum = 100000000
	f.Controls.Add (nudEnd)

	#Set Total checkbox
	chkSetTotal = CheckBox()
	chkSetTotal.AutoSize = True
	chkSetTotal.Location = Point(15, 79)
	chkSetTotal.Size = Size(117, 17)
	chkSetTotal.TabIndex = 2
	chkSetTotal.Text = ComicRack.Localize("Autonumber", "chkTotal", "Save total Number:")
	chkSetTotal.UseVisualStyleBackColor = True
	f.Controls.Add (chkSetTotal)

	#Set Combobox
	cbSelection = ComboBox()
	cbSelection.DropDownStyle = ComboBoxStyle.DropDownList
	cbSelection.FormattingEnabled = True
	cbSelection.Items.Add(ComicRack.Localize("Autonumber", "NumberSeries", "Number Series"))
	cbSelection.Items.Add(ComicRack.Localize("Autonumber", "NumberAltSeries", "Number Alternate Series"))
	cbSelection.Location = Point(13, 14)
	cbSelection.Size = Size(217, 21)
	cbSelection.TabIndex = 6
	f.Controls.Add(cbSelection)
	
	#OK Button
	btOk = Button()
	btOk.DialogResult = DialogResult.OK
	btOk.Location = Point(65, 118)
	btOk.Size = Size(80, 24)
	btOk.TabIndex = 4
	btOk.Text = ComicRack.Localize("Autonumber", "OK", "&OK")
	f.Controls.Add (btOk)
	
	#Cancel button
	btCancel = Button()
	btCancel.DialogResult = DialogResult.Cancel
	btCancel.Location = Point(151, 118)
	btCancel.Size = Size(80, 24)
	btCancel.TabIndex = 5
	btCancel.Text = ComicRack.Localize("Autonumber", "Cancel", "&Cancel")
	f.Controls.Add (btCancel)

	#Setup the form
	f.AcceptButton = btOk
	f.CancelButton = btCancel
	f.AutoScaleMode = AutoScaleMode.Font
	f.ClientSize = Size(243, 154)
	f.FormBorderStyle = FormBorderStyle.FixedDialog
	f.MaximizeBox = False
	f.MinimizeBox = False
	f.ShowIcon = False
	f.ShowInTaskbar = False
	f.StartPosition = FormStartPosition.CenterParent
	f.Text = ComicRack.Localize("Autonumber", "Caption", "Autonumber Wizard")
	
	#Select the proper values
	start = GetStart(books[0].ShadowNumber)
	cbSelection.SelectedIndex = 0
	nudStart.Value = int(start)
	nudEnd.Value = int(start) + books.Length -1
	
	f.AutoScaleDimensions = SizeF(6, 13)
	f.AutoScaleMode = AutoScaleMode.Font
	f.AutoSize = True

	ThemeMe(f)
	f.ResumeLayout(False)
	f.PerformLayout()

	#Show Window
	try:
		if f.ShowDialog(ComicRack.MainWindow)==DialogResult.OK:
		
			n = nudStart.Value
			t = nudEnd.Value
			withTotal = chkSetTotal.Checked
			alternate = cbSelection.SelectedIndex != 0
			
			for book in books:
			
				if alternate:
					book.AlternateNumber = str(n)
					if withTotal:
						book.AlternateCount = t
				else:
					book.Number = str(n)
					if withTotal:
						book.Count = t
						
				n = n + 1
	finally:	
		f.Dispose()

def ThemeMe(control):
    if ComicRack.App.ProductVersion >= '0.9.182':
            ComicRack.Theme.ApplyTheme(control)