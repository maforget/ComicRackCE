<div align="center">

# ComicRack Community Edition
[![ComicRack Community Edition](https://github.com/maforget/ComicRackCE/assets/11904426/4748925c-662f-4ccd-bfb7-62ec46ae881e)](#readme)

[![installation instructions](https://img.shields.io/github/v/release/maforget/ComicRackCE?include_prereleases&label=Download&style=for-the-badge&logo=github)](#installation)
</div>
This project aims to revive the legendary Comic Manager, ComicRack, which hasn't been updated in 10 years. Despite attempts to contact the original author, Markus Eisenstöck, the app was removed from stores, and users had to rely on cracked versions. If Markus reappears and requests this project to be taken down, I will comply.

To support the community, I am releasing the decompiled version as a Community Edition. Although commercial use can't be prevented, I discourage rebranding and selling it. Don't expect major changes or rewrites; this is a work in progress, and contributions from the community are essential. As a hobbyist programmer, I recommend you implement desired features yourself.

You can find the current changelog [here.](https://raw.githubusercontent.com/maforget/ComicRackCE/master/ComicRack/Changes.txt)
## Community Collaboration

To collaborate, open an Issue on the tracker and use GitHub discussions. Start with small, focused Pull Requests, avoiding large, vague commits. ChatGPT can help with small code snippets but not with complete rewrites. Use Visual Studio 2022 Community Edition for development, as it's more suitable than VS Code.

We also need the Localizer tool created by cYo for translations.

# Installation
>[!important]
>This build is unstable and may change daily. Bugs from the decompiling process may still be present, so proceed with caution when upgrading. There have been reports of false positives from Windows Defender, and we cannot submit every version for removal.

To install, download the [nightly installer](https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCESetup_nightly.exe "Nightly Release"), double-click it, and follow the instructions. 
>[!TIP]
>**Uninstall the original ComicRack before installing the Community Edition to prevent duplicates in the Open With menu.**


# Plugins

Check out my plugins for ComicRack Community Edition:

- ~**[Android Client](https://github.com/maforget/ComicRackKeygen/releases/tag/1.0)**: Works with the Community Edition and Android 14 (requires ADB installation for 14+). Includes a Keygen and Support Pack for the original ComicRack, but these are~ no longer needed for the Community Edition.
- **[Amazon Scraper](https://github.com/maforget/ComicRack_AmazonScrapper)**: Scrapes data from Amazon books (formerly Comixology library).
- **[Data Manager](https://github.com/maforget/CRDataManager)**: Lets you manipulate ComicRack data and fixes bugs from the latest v2 release.
- **[Backup Manager](https://github.com/maforget/cr-backup-manager)**: Automates the backup of the ComicRack library file, supporting the Community Edition, Portable mode, and alternate configurations.
- **[MangaUpdate Mini Scraper](https://github.com/maforget/ComicRack_MangaUpdateScraper)**: A mini scraper for mangaupdates.com, fetching genres (additional fields can be enabled manually).
- **[Bédéthèque Scraper v2](https://github.com/maforget/Bedetheque-Scrapper-2)**: Scrapes data from the French BD site Bédéthèque.
- **[Find Image Resolution](https://github.com/maforget/ComicRack_FindImageResolution)**: Determines the resolution of a comic. Right-click => Automation => Find Image Resolution (.NET). Configuration is in File => Automation => Find Image Resolution (.NET) Config.
- **[fullscreen.py](https://gist.githubusercontent.com/maforget/186a99205140acd3f7d3328ad1466e62/raw/8c7c0ecab28fb9a6037adbe19ff553e3597cccd6/fullscreen.py)**: Automatically fullscreens the application when opening a book or starting the app (depending on your settings). Copy the file to `%programfiles%\ComicRack Community Edition\Scripts` or `%appdata%\cYo\ComicRack Community Edition\Scripts`.
- **[comicrack-copy-move-field](https://github.com/maforget/comicrack-copy-move-field)**: Moves or copies info from one field to another, either replacing or appending to the destination field. Updates include support for copying or moving dates.

**You should also consider installing the [ComicVine Scraper](https://github.com/cbanack/comic-vine-scraper/releases/latest) plugin and the [Library Organizer](https://github.com/Stonepaw/comicrack-library-organizer/releases/latest).**
