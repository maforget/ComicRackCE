; Define version and setup filename with iscc.exe /DMyAppVersion=v1.0 /DMyAppSetupFile=ComicRackSetup_v1.0 Installer.iss
#define MyAppName "ComicRack Community Edition"
#ifndef MyAppVersion
#define MyAppVersion "v0.9.178"
#endif
#ifndef MyAppSetupFile
#define MyAppSetupFile "ComicRackSetup"
#endif
#define MyAppPublisher "ComicRack Community"
#define MyAppURL "https://github.com/maforget/ComicRackCE"
#define MyAppExeName "ComicRack.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0FA63C63-846C-49B7-9A4B-553EF8EBEF0B}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ChangesAssociations=yes
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=ComicRack\bin\Release\License.txt
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=.
OutputBaseFilename={#MyAppSetupFile}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
AlwaysShowComponentsList=yes
ArchitecturesInstallIn64BitMode=x64
SetupIconFile=ComicRack\Icons\uninst_103.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName}

[Messages]
// define wizard title and tray status msg
// both are normally defined in innosetup's default.isl (install folder)
SetupAppTitle = {#MyAppName} {#MyAppVersion} Setup
SetupWindowTitle = {#MyAppName} {#MyAppVersion} Setup 

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

; Options displayed during setup (component selection)
[Types]
Name: "full";    Description: "Full installation";
Name: "typical"; Description: "Typical installation";
Name: "compact"; Description: "Compact installation";
Name: "custom";  Description: "Custom installation"; Flags: iscustom

; The compotent definition
[Components]
Name: "app";       Description: "ComicRack Community Edition (Required)";   Types: full typical compact custom; Flags: fixed
Name: "start_menu";Description: "Start Menu";                               Types: full typical
Name: "desktop";   Description: "Desktop Shortcut";                         Types: full typical
Name: "associate"; Description: "Associate eComic extensions";              Types: full typical
Name: "languages"; Description: "Language Packs";                           Types: full
Name: "additional";Description: "Additional images, icons and backgrounds"; Types: full

[Files]
Source: "ComicRack\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\Changes.txt"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\{#MyAppExeName}.config"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\ComicRack.ini"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\DefaultLists.txt"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\License.txt"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\NewsTemplate.html"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\ReadMe.txt"; DestDir: "{app}"; Flags: ignoreversion isreadme; Components: app
Source: "ComicRack\bin\Release\Help\*"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\Languages\*"; DestDir: "{app}\Languages"; Flags: ignoreversion; Components: languages
Source: "ComicRack\bin\Release\Resources\*"; DestDir: "{app}\Resources"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\Resources\Icons\*"; DestDir: "{app}\Resources\Icons"; Flags: ignoreversion; Components: additional
Source: "ComicRack\bin\Release\Resources\Textures\*"; DestDir: "{app}\Resources\Textures"; Flags: ignoreversion recursesubdirs; Components: additional
Source: "ComicRack\bin\Release\Scripts\*"; DestDir: "{app}\Scripts"; Flags: ignoreversion; Components: app
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
; Comics
Root: HKA; Subkey: "Software\Classes\ComicRack.eComic";                       ValueType: string; ValueData: "eComic"; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\ComicRack.eComic\DefaultIcon";           ValueType: string; ValueData: """{autopf}\{#MyAppName}\{#MyAppExeName}"",1"
Root: HKA; Subkey: "Software\Classes\ComicRack.eComic\shell";                 ValueType: string; ValueData: "open"
Root: HKA; Subkey: "Software\Classes\ComicRack.eComic\shell\open";            ValueType: string; ValueData: "Open eComic with ComicRack"
Root: HKA; Subkey: "Software\Classes\ComicRack.eComic\shell\open\command";    ValueType: string; ValueData: """{autopf}\{#MyAppName}\{#MyAppExeName}""  ""%1"""

; Comic Lists
Root: HKA; Subkey: "Software\Classes\ComicRack.ComicList";                    ValueType: string; ValueData: "eComic List"
Root: HKA; Subkey: "Software\Classes\ComicRack.ComicList\DefaultIcon";        ValueType: string; ValueData: """{autopf}\{#MyAppName}\{#MyAppExeName}"",2"
Root: HKA; Subkey: "Software\Classes\ComicRack.ComicList\shell";              ValueType: string; ValueData: "open"
Root: HKA; Subkey: "Software\Classes\ComicRack.ComicList\shell\open";         ValueType: string; ValueData: "Import eComic List into ComicRack CE"
Root: HKA; Subkey: "Software\Classes\ComicRack.ComicList\shell\open\command"; ValueType: string; ValueData: """{autopf}\{#MyAppName}\{#MyAppExeName}"" -il ""%1"""

; ComicRack Plugins
Root: HKA; Subkey: "Software\Classes\ComicRack.Plugin";                       ValueType: string; ValueData: "ComicRack Plugin"
Root: HKA; Subkey: "Software\Classes\ComicRack.Plugin\DefaultIcon";           ValueType: string; ValueData: """{autopf}\{#MyAppName}\{#MyAppExeName}"",3"
Root: HKA; Subkey: "Software\Classes\ComicRack.Plugin\shell";                 ValueType: string; ValueData: "open"
Root: HKA; Subkey: "Software\Classes\ComicRack.Plugin\shell\open";            ValueType: string; ValueData: "Install Plugin into ComicRack CE"
Root: HKA; Subkey: "Software\Classes\ComicRack.Plugin\shell\open\command";    ValueType: string; ValueData: """{autopf}\{#MyAppName}\{#MyAppExeName}"" -ip ""%1"""

; Extensions
Root: HKA; Subkey: "Software\Classes\.cbz";                      ValueType: string; ValueData: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbz\OpenWithProgIDs";      ValueType: string; ValueName: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbr";                      ValueType: string; ValueData: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbr\OpenWithProgIDs";      ValueType: string; ValueName: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cb7";                      ValueType: string; ValueData: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cb7\OpenWithProgIDs";      ValueType: string; ValueName: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbt";                      ValueType: string; ValueData: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbt\OpenWithProgIDs";      ValueType: string; ValueName: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbw";                      ValueType: string; ValueData: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbw\OpenWithProgIDs";      ValueType: string; ValueName: "ComicRack.eComic"
Root: HKA; Subkey: "Software\Classes\.cbl";                      ValueType: string; ValueData: "ComicRack.ComicList"
Root: HKA; Subkey: "Software\Classes\.cbl\OpenWithProgIDs";      ValueType: string; ValueName: "ComicRack.ComicList"
Root: HKA; Subkey: "Software\Classes\.crplugin";                 ValueType: string; ValueData: "ComicRack.Plugin"
Root: HKA; Subkey: "Software\Classes\.crplugin\OpenWithProgIDs"; ValueType: string; ValueName: "ComicRack.Plugin"

; Application specific
Root: HKA; Subkey: "Software\Microsoft\Windows\CurrentVersion\App Paths\{#MyAppExeName}"; ValueType: string; ValueData: "{autopf}\{#MyAppName}\{#MyAppExeName}"
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\shell\open\command";    ValueType: string; ValueData: """{autopf}\{#MyAppName}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes";        ValueType: string; ValueName: ".cb7"; ValueData: ""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".cbz"; ValueData: ""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".cbr"; ValueData: ""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".cbt"; ValueData: ""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".cbw"; ValueData: ""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".cbl"; ValueData: ""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".crplugin"; ValueData: ""

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}";                Components: start_menu
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}";     Components: start_menu
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; Components: start_menu
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}";          Components: desktop

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

