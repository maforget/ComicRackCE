# CommitProposed.py
#
# Writes the proposed values (generated from file names) to the real values of the comic
#
# You are free to modify and distribute this file
# (C)'2007 cYo Soft
##########################################################################

#@Name	Commit Proposed Values...
#@Hook	Books
#@Description A script to makes the proposed values for the Books permanent
def CommitProposed(books):

	result = ComicRack.App.AskQuestion (
			ComicRack.Localize("CommitProposed", "AskCommit", "Do you really want to make the proposed values permanent?\nProposed values are generated from the filename and are displayed in gray"), 
			ComicRack.Localize("CommitProposed", "Write", "Write"), 
			ComicRack.Localize("CommitProposed", "Overwrite", "&Also overwrite existing values"))
	if result != 0:
		for book in books:
			if book.EnableProposed:
				book.WriteProposedValues(result == 2)
			
