This folder can contain a list of ini files and documents for the help system of ComicRack.
The structure of such ini files is as follows:

1) General variables
--------------------

$APPEXE		-	ComicRack itself
$APPPATH	-	Path to the install folder
$APPDATA	-	Path to the ComicRack data folder
$USERPATH	-	Path to the user home folder


2) Defining the application to use: HELPAPP
-------------------------------------------

If a special application is needed to start the links it can be defined with the HelpApp key.
Possible values are a path or a registry key. If not specified the help link is a internet address.

Examples:
HelpApp = $APPEXE
HelpApp = HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Acrobat.exe

3) Defining the help link: HELPLINK
-----------------------------------

If there is a pattern to the external links, a HelpLink can be defined. The {0} will be
subsitituted with the value of the actual mapping.
The link can either either define the commandline parameters for the application or the internet url.

Examples:
HelpLink = /A "page={0}" "$APPPATH\Help\manual.pdf"
HelpLink = http://comicrack.cyolito.com/documentation/wiki?id={0}


3) Defining the Main Menu Entries
---------------------------------

You can replace the default link for the ComicRack Documentation with an entry names HelpMain.

Example:
HelpMain=http://comicrack.cyolito.com

It is also possible to add additional menu entries to the help system (e.g. direct jumps to chapters).

Example:
HelpMenu1=Open Google; http://google.com
HelpMenu2=Visit this!; http://visitblabla.com

5) The mappings:
----------------

Each ComicRack context key is mapped to a value. This value is passed with the helplink by replacing
{0} with the actual value.

Examples:
MainForm:readerContainer = gui:reader
MainForm:readerContainer = 5

For a full list of ComicRack context keys seek "ComicRack Wiki.ini"


6) Tips and tricks:
-------------------

If a mapping or link starts with like a file or internet url (like c:\ or http:// - this also includes
$APPEXE substitutions) HelpLink and HelpApp defintions are not used and the link is directly called.

If you want to call Adobe Acrobat Reader to open a PDF file on a specific page use the following settings:

HelpApp = HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\AcroRd32.exe
HelpApp2 = HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Acrobat.exe
HelpLink = /A "page={0}" "$APPPATH\Help\manual.pdf" 

If you want to use ComicRack itself to open a documentation:

HelpApp = $APPEXE
HelpLink = "$APPPATH\Help\manual.djvu" -p {0}
