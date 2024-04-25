# ComicRack Community Edition

#### These are automatically build for each commit, so this nightly will always be up to date and is built directly by GitHub, so you know the code you downloaded is the same as the repo. When using the installer, I highly suggest you uninstall the original ComicRack before installing the Community Edition. It will prevent duplicates with the Open With menu.

_Please keep in mind that this build isn't considered stable. It will change multiple times, perhaps daily. There are still some bugs that might have came from the decompiling process, as such please be careful when "upgrading" to this version. There are also some reports of False Positive by Windows Defender. Since this is an ever changing build, we cannot submit all version for removal._

<p align="center">
    <b><u><span style='font-size:14.0pt'>👇 Download Links 👇</span></u></b>
</p>
<p align="center">
<!--
  <a href="https://github.com/maforget/ComicRack_AmazonScrapper/releases/latest/download/AmazonScrapper.crplugin" alt="Latest Release">
      <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?label=latest%20release&logo=github" /></a>
-->
    <a href="https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCE_nightly.zip" alt="Nightly (ZIP)">
      <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?include_prereleases&label=pre-release (zip)&logo=github" /></a>
    <a href="https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCESetup_nightly.exe" alt="Nightly (EXE)">
      <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?include_prereleases&label=pre-release (installer)&logo=github" /></a>
    <br>
      <img src="https://img.shields.io/github/release-date-pre/maforget/ComicRackCE?logo=github&label=Released" /></a>   

</p>

---

<p align="center">
<img src="https://github.com/maforget/ComicRackCE/assets/11904426/4748925c-662f-4ccd-bfb7-62ec46ae881e" />
</p>


This project is an attempt to resurrect the legendary Comic Manager ComicRack from the dead. It's been 10 years since we had an update. We hoped that the original author would be back to it but it never happened. The website went down, the application was taken from the App Store and later the Android Store. Late last year, the Android application even stopped working for people that bought it previously, forcing the use of a cracked version. That meant modifying the desktop client to allow syncing for cracked application. There was multiple attempt to contact him by various people that failed to release or buy the source code. There is also the grey area that this project is still the intellectual property of cYo (Markus Eisenstöck), so if he would give signs of life (with proof) and require that this project be taken down, I will acknowledge his request.

So in the spirit of reviving this program for the community, I decided to release the decompiled I had. I really want this to be a Community Edition. By the Community for the Community. Although no License can prevent Commercial Use, I really don't want someone rebranding it as their own and selling it, that would be really bad.

I want to temper expectations, don't expect some sweeping changes, like a complete rewrite of all the program and UI. There are plenty of things that needs doing, but don't just expect for me to do them all. I am also just a hobbyist programmer and can understand most of the code with enough time to follow it. If you really want a new feature, than I suggest that the best is to try to implement it yourself. This is why it's called Community Edition, because it should be a work by and for the community. This is very much a work in progress, and please expect update very often.

For anyone that wants to cooperate, start by opening a Issue in the tracker so we can all know what you are working on. There is also a discussion are here on GitHub you can use. I would start by doing some Pull Request. I would ask that you keep your commit small and just to 1 change. Not commits like "changed stuff" with changes to 150 files. This is a BIG code base, so knowing what you changed easily is important. You can have multiple commits for 1 PR.

Also for all the ChatGPT fans out there, it can be helpful, but in small snippets of code. Don't expect it to rewrite the whole program for you. And if you want to understand what does what, then just use the debugging function of Visual Studio. Speaking of please stick to Visual Studio 2022 Community Edition. VS Code isn't at the same level (but that is your choice).

We will need the Localizer tool that cYo did to help translate the program. 

See the current changelog [here.](https://raw.githubusercontent.com/maforget/ComicRackCE/master/ComicRack/Changes.txt)

## Installation

To install, download the [nightly installer](https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCESetup_nightly.exe "Nightly Release"), double-click the installer provided and follow the instructions. When using the installer, I highly suggest you uninstall the original ComicRack before installing the Community Edition. It will prevent duplicates with the Open With menu. It will still work correctly even if you have both together.

## Upgrading from classic ComicRack

ComicRack Community Edition uses a different data directory than classic ComicRack. In order to transfer your data over:

1. Run ComicRack Community Edition from the link on your Desktop or Start Menu.
2. Once ComicRack has loaded, close it again (check the system tray), this is to create the required folders.
3. Open Windows Explorer.
2. Copy and paste `%appdata%\cYo\ComicRack` into the address bar and press enter. You'll see your existing ComicRack data. Consider making a backup of this directory just in case.
3. Copy all of the contents of this directory.
4. Navigate one directory up and open the `ComicRack Community Edition` directory.
5. Paste your data into this directory and overwrite any existing files.
6. Open ComicRack Community Edition and all your comics/scripts should be there.

Remember that changes in one, will not carry over to the other.

---

Check the [Wiki](https://github.com/maforget/ComicRackCE/wiki) for additional help & guides, such as info about using the Android Client. The wiki is open for contributor to add to.

----

#### Check my plugins for ComicRack Community Edition:

- **[Android Client](https://github.com/maforget/ComicRackKeygen/releases/tag/1.0). Includes the link to the Android Client. It still work correctly with the Community Edition & still works with the Latest Android 14 (although for 14+ you will need to install via ADB). Also includes stuff like a Keygen & a Support Pack for the Original ComicRack, but those aren't required anymore with the Community Edition anymore.**
- **[Amazon Scrapper](https://github.com/maforget/ComicRack_AmazonScrapper). A Scrapper for Amazon books (former Comixology library).**
- **[Data Manager](https://github.com/maforget/CRDataManager) let's you manipulate your data for ComicRack. Fix the various bugs in the latest v2 release.**
- **[Backup Manager for ComicRack](https://github.com/maforget/cr-backup-manager) Automates the process of saving the ComicRack library file or easy 1 click backup. Updated to support the Community Edition, Portable mode & Alternate Configurations.**
- **[MangaUpdate Mini Scraper](https://github.com/maforget/ComicRack_MangaUpdateScraper) A mini scraper (no gui, only fetches genres) for mangaupdates.com (more fields can be enabled manually).**
- **[Bédéthèque Scraper v2](https://github.com/maforget/Bedetheque-Scrapper-2) to scrap data from the French BD site Bédétheque.**
- **[Find Image Resolution](https://github.com/maforget/ComicRack_FindImageResolution) to determine the resolution of a comic. Use it by right-clicking => Automation => Find Image Resolution (.NET). Configuration are in File => Automation => Find Image Resolution (.NET) Config.**
- **[fullscreen.py](https://gist.githubusercontent.com/maforget/186a99205140acd3f7d3328ad1466e62/raw/8c7c0ecab28fb9a6037adbe19ff553e3597cccd6/fullscreen.py). It will automatically fullscreen the application when either opening a book or starting the application depending on which you enable). Copy the file in either `%programfiles%\ComicRack Community Edition\Scripts` or `%appdata%\cYo\ComicRack Community Edition\Scripts`.**
- **[comicrack-copy-move-field](https://github.com/maforget/comicrack-copy-move-field). Moves or copies info from one field to another. Can either replace or append to the destination field. Small update from the original to permit dates to be copied or moved.**

 ##### You should also consider installing the [ComicVine Scrapper](https://github.com/cbanack/comic-vine-scraper/releases/latest) plugin & [Library Organizer](https://github.com/Stonepaw/comicrack-library-organizer/releases/latest)
