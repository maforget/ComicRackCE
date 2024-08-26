<div align="center">

# ComicRack Community Edition

<b><u><span style='font-size:14.0pt'>👇 Download Links 👇</span></u></b>

<!--
<p>
<a href="https://github.com/maforget/ComicRack_AmazonScrapper/releases/latest/download/ComicRackCESetup.zip" alt="Latest Release (ZIP)">
  <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?label=latest release&logo=github" /></a>
<a href="https://github.com/maforget/ComicRack_AmazonScrapper/releases/latest/download/ComicRackCESetup.exe" alt="Latest Release (EXE)">
  <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?label=latest release (installer)&logo=github" /></a> 
<br>
  <img src="https://img.shields.io/github/release-date/maforget/ComicRackCE?logo=github&label=Released" /></a>    
</p>
-->

<p>
<a href="https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCE_nightly.zip" alt="Nightly (ZIP)">
  <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?include_prereleases&label=pre-release (zip)&logo=github" /></a>
<a href="https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCESetup_nightly.exe" alt="Nightly (EXE)">
  <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?include_prereleases&label=pre-release (installer)&logo=github" /></a>
<br>
  <img src="https://img.shields.io/github/release-date-pre/maforget/ComicRackCE?logo=github&label=Released" /></a>   
</p>

[![ComicRack Community Edition](https://github.com/maforget/ComicRackCE/assets/11904426/4748925c-662f-4ccd-bfb7-62ec46ae881e)](#readme)
</div>

---

This project aims to revive the legendary Comic Manager, ComicRack, which hasn't been updated in 10 years. Despite attempts to contact the original author, Markus Eisenstöck (cYo), the app was removed from stores, and users had to rely on cracked versions. If Markus reappears and requests this project to be taken down, I will comply.

To support the community, I am releasing the decompiled version as a Community Edition. Although commercial use can't be prevented, I discourage rebranding and selling it. Please keep expectations realistic; major overhauls, like rewriting the entire program or UI, aren't on the horizon. As a hobbyist programmer, I can handle most code but with time. If you're eager for a new feature, I encourage you to consider implementing it yourself—it's what makes this the Community Edition.

New Features are listed [here](https://github.com/maforget/ComicRackCE/wiki/New-Features). The complete changelog is [here](https://raw.githubusercontent.com/maforget/ComicRackCE/master/ComicRack/Output/Changes.txt).

## Community Collaboration
To collaborate, open an Issue on the tracker or use GitHub discussions. Start with small, focused Pull Requests, avoiding large, vague commits. ChatGPT can help with small code snippets but not with complete rewrites. Use Visual Studio 2022 Community Edition for development, as it's more suitable than VS Code.

We also need the Localizer tool created by cYo for translations.

## Installation
To install, download the [nightly installer](https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCESetup_nightly.exe "Nightly Release"), double-click it, and follow the instructions. 

> [!CAUTION]
> Because of a change with Microsoft Visual C++ Redistributable 2015-2022, if you use the HEIF/AVIF files you will need to have at a minimum version `14.40.33810.0` installed. This should be done automatically by the installer, but if you are using the ZIP file, please be advise that you will need to update it manually. More info [here](https://github.com/maforget/ComicRackCE/issues/106).
>
> Link to lastest Visual C++ Redistributable 2015-2022: https://aka.ms/vs/17/release/vc_redist.x64.exe

>[!IMPORTANT]
>* This build is currently under development and may undergo daily changes. Bugs resulting from the decompiling process may still exist, so exercise caution when upgrading.
>* Users have reported false positives from Windows Defender and not every version can be submitted for removal.
>    * Builds provided are automatically compiled by GitHub servers, ensuring that the downloaded file matches the code in this repository.

>[!WARNING]
>* This version introduces new smart list fields not found in classic ComicRack. Do not open a database that utilizes these fields in an older version, as it will reset your database.
>* Always maintain backups.

>[!TIP]
>* Before installing the Community Edition, uninstalling the original ComicRack is suggested to avoid duplicates in the Open With menu.
>* An updated version of the [Backup Manager](https://github.com/maforget/cr-backup-manager) is provided to help you backup your database.
>* Use the News window within the application to stay informed about the latest builds.
>* Check the [ComicRackCE wiki](https://github.com/maforget/ComicRackCE/wiki) for additional tips and information.

### Upgrading from classic ComicRack

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

## Plugins

Check out my plugins for ComicRack Community Edition:

- **[Android Client](https://github.com/maforget/ComicRackKeygen/releases/tag/1.0)**: Works with the Community Edition and Android 14 (requires ADB installation for 14+). ~Includes a Keygen and Support Pack for the original ComicRack, but these are~ (no longer needed for the Community Edition).
- **[Amazon Scraper](https://github.com/maforget/ComicRack_AmazonScrapper)**: Scrapes data from Amazon books (formerly Comixology library).
- **[Data Manager](https://github.com/maforget/CRDataManager)**: Lets you manipulate ComicRack data and fixes bugs from the latest v2 release.
- **[Backup Manager](https://github.com/maforget/cr-backup-manager)**: Automates the backup of the ComicRack library file, supporting the Community Edition, Portable mode, and alternate configurations.
- **[MangaUpdate Mini Scraper](https://github.com/maforget/ComicRack_MangaUpdateScraper)**: A mini scraper for mangaupdates.com, fetching genres (additional fields can be enabled manually).
- **[Bédéthèque Scraper v2](https://github.com/maforget/Bedetheque-Scrapper-2)**: Scrapes data from the French BD site Bédéthèque.
- **[Find Image Resolution](https://github.com/maforget/ComicRack_FindImageResolution)**: Determines the resolution of a comic. Right-click => Automation => Find Image Resolution (.NET). Configuration is in File => Automation => Find Image Resolution (.NET) Config.
- **[fullscreen.py](https://gist.githubusercontent.com/maforget/186a99205140acd3f7d3328ad1466e62/raw/8c7c0ecab28fb9a6037adbe19ff553e3597cccd6/fullscreen.py)**: Automatically fullscreens the application when opening a book or starting the app (depending on your settings). Copy the file to `%programfiles%\ComicRack Community Edition\Scripts` or `%appdata%\cYo\ComicRack Community Edition\Scripts`.
- **[comicrack-copy-move-field](https://github.com/maforget/comicrack-copy-move-field)**: Moves or copies info from one field to another, either replacing or appending to the destination field. Updates include support for copying or moving dates.

**You should also consider installing the [ComicVine Scraper](https://github.com/cbanack/comic-vine-scraper/releases/latest) plugin and the [Library Organizer](https://github.com/Stonepaw/comicrack-library-organizer/releases/latest).**
