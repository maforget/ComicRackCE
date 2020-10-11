# Sample.py
#
# Includes some sample scripts:
#   SaveCSVList - Writes some book properties as a csv file.
#   RenameBookFiles - Renames the book files to a default name pattern
#   ParseComicPath - Sample parser for proposed comic values
#   BookHasBeenOpened - Sample script that gets called whenever a comic is opened
#   GetBooksWith - A sample smartlist script
#   AutoadjustTwoPageMode - Switches Two Page mode based on reader size 
#
# You are free to modify and distribute this file
#
##########################################################################

import clr
import sys
from cYo.Projects.ComicRack.Engine.Display import *
from System.Collections.Generic import *

clr.AddReferenceByPartialName("System.Windows.Forms")
clr.AddReferenceByPartialName("System.Drawing")
from System.Windows.Forms import *
from System.Drawing import *

#
# Sample Parser for Book paths
#
# path : string containing the full path of the Book
# proposed : object with properties Series/Volume/Number/Count/Year
#
# the values in proposed are prefilled with the guessed ComicRack made.
# You can change these values or replace them as you like.
#
def ParseComicPath (path, proposed):
	proposed.Series = "-" + proposed.Series
	proposed.Title = "This is a sample title"
	proposed.Volume = 1
	proposed.Number = "1.5"
	proposed.Count = 56
	proposed.Year = 2000
	proposed.Format = "Annual"
	proposed.CoverCount = 1

#
# Sample handler for the opening a new book event
#
# could be used for something like sending a message to
# an irc chat or twitter or facebook status update
#	
def BookHasBeenOpened (book):
	MessageBox.Show ("opened " + book.Caption)
	
#
# Sample handler for Configuring BookOpen
#
# Hook must be ConfigScript, Key must be same as the script to config
#
#@Key   BookHasBeenOpened
#@Hook  ConfigScript
def ConfigureBookHasBeenOpened ():
	MessageBox.Show ("Should open a window to configure the script!")
	
#
# A simple script generate book lists
#
# Methods with hook CreateBookList are selectable in the
# smartlist editor. 
#
# books is the input list of books from ComicRack
# a is the first paramter from the smartlist editor
# b is the second parameter
#
# the method should return a list/array of books
# that are a subset of the input list
#
# This could/should be used to generate very sophisticated smartlists
# (like finding missing issues in sequences etc.)
#
#@Name	 [Code Sample] All Books starting with
#@Hook	 CreateBookList
#@PCount 1
#@Enabled false
#@Description Sample script to show how to write custom smartlist scripts
def GetBooksWith (books, a, b):
	newList = []	
	for book in books:
		if book.ShadowSeries.startswith(a):
			newList.append(book)
	return newList

#
# A simple script to auto adjust reader settings during resize
#
#@Name [Code Sample] Autoadjust Two Page Mode
#@Hook ReaderResized
#@Enabled false
#@Description Sample script that shows how to dynamically change viewer properties
def AutoadjustTwoPageMode(width, height):
	twoPage = width > height
	ComicRack.ComicDisplay.TwoPageDisplay = twoPage
	if twoPage:
		ComicRack.ComicDisplay.PageTransitionEffect = PageTransitionEffect.Paging
	else:
		ComicRack.ComicDisplay.PageTransitionEffect = PageTransitionEffect.Fade

#
# A simple script to add a new search engine
#
# A search script returns a dctionary of [name,url] type
# limit shows how much items should be returned max
# if limit is -1, a generic search url should be returned
#
#@Name [Code Sample] Dummy Search
#@Hook NetSearch
#@Enabled false
#@Description A simple script for showing how Search scripts are done
def DummySearch(hint, text, limit):
	d = Dictionary[str, str]()
	if limit == -1:
		d.Add("General", "http://google.com/search?q=")
	else:
		for n in range(1, limit):
			d.Add("Result " + str(n), "http://google.com/search?q=" + text)
	return d	

#
# A simple script to add a new comic info html panel
#
# the script should return some renderable html code (can include external references)
# also demonstrates calling back from html into comicrack
#
#@Name [Code Sample] Dummy Book Info HTML
#@Hook ComicInfoHtml
#@Enabled false
#@Description A simple script to show how html info panels are done
def DummyHtmlInfoPanel(books):
	s = '<table>'
	for b in books:
		s += '<tr>'
		s += '<td>' + b.Caption + '</td>'
#		s += '<td><button onClick="window.external.Config += \'!\'">Open</button></td>'
		s += '<td><button onClick="window.external.ComicRack.OpenBooks.OpenFile(\'id:' + b.Id.ToString() + '\', true, 0)">Open</button></td>'
		s += '</tr>'
	s += '</table>'
	return s

#
# A simple script to display the web link content
#
# You can also return a url when the string starts with "!"
#
#@Name Web link
#@Hook ComicInfoHtml
#@Enabled false
#@Description A script to display the web link content
def WebLinkInfoPanel(books):
    for b in books:
		if b.Web != "":
			return "!" + b.Web

#
# A simple script to add a new comic info ui panel
#
# the script is only called once and should return a Windows Forms Control
# the control must implement the method ShowInfo(books)
#

# this is our UI control
# note the ShowInfo implementation
class UIControl(Control):
	def __init__(self):
		self.lb = ListBox()
		self.lb.Dock = DockStyle.Fill
		self.lb.IntegralHeight = False
		self.Controls.Add(self.lb)   
        
	def ShowInfo(self, books):
		print 'Calling...' + str(len(books))
		self.lb.Items.Clear()
		for b in books:
			print 'Adding ' + b.Caption
			self.lb.Items.Add(b.Caption)

#@Name [Code Sample] Dummy Book Info UI
#@Hook ComicInfoUI
#@Enabled false
#@Description A simple script to show how ui info panels are done
def DummyUIInfoPanel():
    return UIControl()

#
# A simple script to display the ComicRack User Forum in the quick Open
#
# You can also return a url when the string starts with "!"
# The books parameter is the list of books displayed in the default quick view
#
#@Name [Code Sample] Show ComicRack User Forum
#@Hook QuickOpenHtml
#@Enabled false
#@Description A script to display the ComicRack User Forum in Quick Open
def ComicRackUserForum(books):
	return "!http://comicrack.cyolito.com/forum/recent"
    
#
# A simple script to fade thumbnails for comics already read
#
# This script is called after the comic thumbnail has been drawn
# Parameters are the book, the GDI+ graphics object, the thumbnail bounds,
# flags is a bit-mask with:
#
#	1..	Hot
#	2.. Selected
#   4.. Stacked
#
#@Name [Code Sample] Fade read comic thumbnails
#@Hook DrawThumbnailOverlay
#@Enabled false
#@Description A simple script to show how thumbnail overlays are drawn
def FadeReadThumbnails(book, graphics, bounds, flags):
	if flags != 0 or book.ReadPercentage < 80:
		return

	br = SolidBrush(Color.FromArgb(128, 255, 255, 255))
	graphics.FillRectangle(br, bounds)
	br.Dispose()


#
# A simple script to display a startup message
#
# This script is called after ComicRack has started
#
#@Name [Code Sample] Display startup message
#@Hook Startup
#@Enabled false
#@Description A simple script to show how to execute a script on startup
def ShowStartupMessage():
	MessageBox.Show ("ComicRack started")
    
#
# A simple script to display a shutdown message
#
# This script is called after ComicRack has started
#
#@Name [Code Sample] Display shutdown message
#@Hook Shutdown
#@Enabled false
#@Description A simple script to show how to execute a script on shutdown. Return false to abort shutdown
def ShowShutdownMessage(user_is_closing):
	return MessageBox.Show ("ComicRack will shutdown! Do you want to continue?", "Shutdown Script", MessageBoxButtons.YesNo) == DialogResult.Yes
    