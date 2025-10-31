# NewComics.py
#
# Shows a dialog to add a range of new books
#
# You are free to modify and distribute this file
# (C)'2010 cYo Soft
##########################################################################

import clr
clr.AddReferenceByPartialName("System.Windows.Forms")
clr.AddReferenceByPartialName("System.Drawing")

import System.Drawing
import System.Windows.Forms

from System.Drawing import *
from System.Windows.Forms import *

class NewComicsForm(Form):
	def __init__(self):
		self.InitializeComponent()
		ThemeMe(self)
	
	def InitializeComponent(self):
		self._labelSeries = System.Windows.Forms.Label()
		self._textSeries = System.Windows.Forms.TextBox()
		self._textVolume = System.Windows.Forms.TextBox()
		self._labelVolume = System.Windows.Forms.Label()
		self._labelFromNumber = System.Windows.Forms.Label()
		self._labelToNumber = System.Windows.Forms.Label()
		self._textToNumber = System.Windows.Forms.TextBox()
		self._btCancel = System.Windows.Forms.Button()
		self._btOK = System.Windows.Forms.Button()
		self._textFromNumber = System.Windows.Forms.TextBox()
		self.SuspendLayout()
		# 
		# labelSeries
		# 
		self._labelSeries.Location = System.Drawing.Point(12, 18)
		self._labelSeries.Name = "labelSeries"
		self._labelSeries.Size = System.Drawing.Size(54, 18)
		self._labelSeries.TabIndex = 0
		self._labelSeries.Text = ComicRack.Localize("NewComics", "labelSeries", "Series:")
		# 
		# textSeries
		# 
		self._textSeries.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
		self._textSeries.Location = System.Drawing.Point(72, 15)
		self._textSeries.Name = "textSeries"
		self._textSeries.Size = System.Drawing.Size(259, 20)
		self._textSeries.TabIndex = 1
		self._textSeries.TextChanged += self.InputTextChanged
		# 
		# labelFromNumber
		# 
		self._labelFromNumber.Location = System.Drawing.Point(12, 47)
		self._labelFromNumber.Name = "labelFromNumber"
		self._labelFromNumber.Size = System.Drawing.Size(54, 18)
		self._labelFromNumber.TabIndex = 2
		self._labelFromNumber.Text = ComicRack.Localize("NewComics", "labelFromNumber", "Number:")
		# 
		# textFromNumber
		# 
		self._textFromNumber.Location = System.Drawing.Point(72, 44)
		self._textFromNumber.Name = "textFromNumber"
		self._textFromNumber.Size = System.Drawing.Size(63, 20)
		self._textFromNumber.TabIndex = 3
		self._textFromNumber.TextChanged += self.InputTextChanged
		# 
		# labelToNumber
		# 
		self._labelToNumber.Location = System.Drawing.Point(130, 46)
		self._labelToNumber.Name = "labelToNumber"
		self._labelToNumber.Size = System.Drawing.Size(45, 18)
		self._labelToNumber.TextAlign = System.Drawing.ContentAlignment.TopCenter
		self._labelToNumber.TabIndex = 4
		self._labelToNumber.Text = ComicRack.Localize("NewComics", "labelToNumber", "to")
		# 
		# textToNumber
		# 
		self._textToNumber.Location = System.Drawing.Point(180, 43)
		self._textToNumber.Name = "textToNumber"
		self._textToNumber.Size = System.Drawing.Size(63, 20)
		self._textToNumber.TabIndex = 5
		self._textToNumber.TextChanged += self.InputTextChanged
		# 
		# labelVolume
		# 
		self._labelVolume.Location = System.Drawing.Point(12, 76)
		self._labelVolume.Name = "labelVolume"
		self._labelVolume.Size = System.Drawing.Size(54, 18)
		self._labelVolume.TabIndex = 6
		self._labelVolume.Text = ComicRack.Localize("NewComics", "labelVolume", "Volume:")
		# 
		# textVolume
		# 
		self._textVolume.Location = System.Drawing.Point(72, 73)
		self._textVolume.Name = "textVolume"
		self._textVolume.Size = System.Drawing.Size(63, 20)
		self._textVolume.TabIndex = 7
		self._textVolume.TextChanged += self.InputTextChanged
		# 
		# btOK
		# 
		self._btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
		self._btOK.DialogResult = System.Windows.Forms.DialogResult.OK
		self._btOK.Enabled = False
		self._btOK.FlatStyle = System.Windows.Forms.FlatStyle.System
		self._btOK.Location = System.Drawing.Point(167, 111)
		self._btOK.Name = "btOK"
		self._btOK.Size = System.Drawing.Size(80, 24)
		self._btOK.TabIndex = 8
		self._btOK.Text = ComicRack.Localize("NewComics", "btOK", "&OK")
		# 
		# btCancel
		# 
		self._btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right
		self._btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		self._btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
		self._btCancel.Location = System.Drawing.Point(253, 111)
		self._btCancel.Name = "btCancel"
		self._btCancel.Size = System.Drawing.Size(80, 24)
		self._btCancel.TabIndex = 9
		self._btCancel.Text = ComicRack.Localize("NewComics", "btCancel", "&Cancel")
		# 
		# NewComicsForm
		# 
		self.AutoScaleDimensions = SizeF(6, 13)
		self.AutoScaleMode = AutoScaleMode.Font
		self.AcceptButton = self._btOK
		self.CancelButton = self._btCancel
		self.ClientSize = System.Drawing.Size(343, 143)
		self.Controls.Add(self._btCancel)
		self.Controls.Add(self._btOK)
		self.Controls.Add(self._textVolume)
		self.Controls.Add(self._textFromNumber)
		self.Controls.Add(self._labelToNumber)
		self.Controls.Add(self._labelFromNumber)
		self.Controls.Add(self._textToNumber)
		self.Controls.Add(self._labelVolume)
		self.Controls.Add(self._textSeries)
		self.Controls.Add(self._labelSeries)
		self.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		self.MaximizeBox = False
		self.MinimizeBox = False
		self.Name = "NewComicsForm"
		self.ShowIcon = False
		self.ShowInTaskbar = False
		self.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		self.Text = ComicRack.Localize("NewComics", "Caption", "New fileless Book Series")
		self.ResumeLayout(False)
		self.PerformLayout()

	def GetStart(self):
		return GetNumber(self._textFromNumber.Text)
	
	def GetEnd(self):
		return GetNumber(self._textToNumber.Text)
	
	def GetSeries(self):
		return self._textSeries.Text

	def GetVolume(self):
		return GetNumber(self._textVolume.Text)

	def InputTextChanged(self, sender, e):
		start = self.GetStart()
		self._btOK.Enabled = (len(self.GetSeries()) > 0) and (start >= 0) and (self.GetEnd() >= start)

def GetNumber(start):
	try:
		return int(start)
	except:
		return -1

#@Name	New fileless Book Series...
#@Key	NewComics
#@Hook	NewBooks
#@Description Adds a new fileless Series of Books
def NewComicBooks(books):
	
	f = NewComicsForm()
	
	try:
		if f.ShowDialog(ComicRack.MainWindow)==DialogResult.OK:
		
			firstNumber = f.GetStart()
			lastNumber = f.GetEnd()
			v = f.GetVolume()

			#Sanity Check
			count = lastNumber - firstNumber
			if count > 100:
				return
		
			books = []

			for n in range(firstNumber, lastNumber+1):
				cb = ComicRack.App.AddNewBook(False)
				cb.Series = f.GetSeries()
				cb.Number = str(n)
				cb.Volume = v
				books.append(cb)

			ComicRack.Browser.SelectComics(books)
    
	finally:	
		f.Dispose()

def ThemeMe(control):
    if ComicRack.App.ProductVersion >= '0.9.182':
            ComicRack.Theme.ApplyTheme(control)
