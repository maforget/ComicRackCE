# ComicRack Community Edition

#### These are automatically build for each commit, so this nightly will always be up to date and is build directly by Github, so you know the code you downloaded is the same than from the repo.

_Please keep in mind that this build isn't considered a stable build, It will change multiple times perhaps daily. There are still some bugs that might have came from the decompiling process, as such please be careful when "upgrading" to this version. There are also some reports of False Positive by Windows Defender, since this is an ever changing build, we cannot submit all version for removal._

<p align="center">
    <b><u><span style='font-size:14.0pt'>👇 Download Links 👇</span></u></b>
</p>
<p align="center">
<!--
  <a href="https://github.com/maforget/ComicRack_AmazonScrapper/releases/latest/download/AmazonScrapper.crplugin" alt="Latest Release">
      <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?label=latest%20release&logo=github" /></a>
-->
    <a href="https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCE_nightly.zip" alt="Nightly">
      <img src="https://img.shields.io/github/v/release/maforget/ComicRackCE?include_prereleases&label=pre-release&logo=github" /></a>
    <br>
    <a href="https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCE_nightly.zip" alt="Nightly">
      <img src="https://img.shields.io/github/release-date-pre/maforget/ComicRackCE?logo=github&label=Released" /></a>

</p>

---

<p align="center">
<img src="https://github.com/maforget/ComicRackCE/assets/11904426/4748925c-662f-4ccd-bfb7-62ec46ae881e" />
</p>


This project is an attempt to resurrect the legendary Comic Manager ComicRack from the dead. It's been 10 years since we had an update. We hoped that the original author would be back to it but it never happened. The website went down, the application was taken from the App Store and later the Android Store. Late last year, the Android application even stopped working for people that bought it previsouly, forcing the use of a cracked version. That meant modifying the desktop client to allow syncing for cracked application. There was multiple attempt to contact him by various people that failed to release or buy the source code. There is also the grey area that this project is still the intellectual property of cYo (Markus Eisenstöck), so if he would give signs of life (with proof) and require that this project be taken down, I will acknowledge his request.

So in the spirit of reviving this program for the community, I decided to release the decompiled I had. I really want this to be a Community Edition. By the Community for the Community. Although no License can prevent Commercial Use, I really don't want someone rebranding it as their own and selling it, that would be really bad.

I want to temper expectations, don't expect some sweeping changes, like a complete rewrite of all the program and UI. There are plenty of things that needs doing, but don't just expect for me to do them all. I am also just a hobbyist programmer and can understand most of the code with enough time to follow it. If you really want a new feature, than I suggest that the best is to try to implement it yourself. This is why it's called Community Edition, because it should be a work by and for the community. This is very much a work in progress, and please expect update very often.

For anyone that wants to cooperate, start by opening a Issue in the tracker so we can all know what you are working on. There is also a discussion are here on Github you can use. I would start by doing some Pull Request. I would ask that you keep your commit small and just to 1 change. Not commits like "changed stuff" with changes to 150 files. This is a BIG code base, so knowing what you changed easily is important. You can have multiple commits for 1 PR.

Also for all the ChatGPT fans out there, it can be helpful, but in small snippets of code. Don't expect it to rewrite the whole program for you. And if you want to understand what does what, then just use the debugging function of Visual Studio. Speaking of please stick to Visual Studio 2022 Community Edition. VS Code isn't at the same level (but that is your choice).

We will need the Localizer tool that cYo did to help translate the program. 

See the current changelog [here.](https://raw.githubusercontent.com/maforget/ComicRackCE/master/ComicRack/Changes.txt)

## Installation

ComicRack Community Edition currently does not have a standard installation package. To install, download the [nightly release](https://github.com/maforget/ComicRackCE/releases/download/nightly/ComicRackCE_nightly.zip "Nightly Release") and follow the instructions below:

1. Extract the nightly release archive to a folder on your computer.
2. Ensure that a previous installation of ComicRack is not currently running (check the system tray).
3. Run ComicRack.exe in the folder you extracted to.
4. Once ComicRack has loaded, close it again (check the system tray).

## Upgrading from classic ComicRack

ComicRack Community Edition uses a different data directory than classic ComicRack. In order to transfer your data over:

1. Open File Explorer.
2. Copy and paste `%appdata%\cYo\ComicRack` into the address bar and press enter. You'll see your existing ComicRack data. Consider making a backup of this directory just in case.
3. Copy all of the contents of this directory.
4. Navigate one directory up and open the ComicRack Community Edition directory.
5. Paste your data into this directory and overwrite any existing files.
6. Open ComicRack.exe from the Community Edition and all your comics should be there.

Remember that changes in one, will not carry over to the other.

