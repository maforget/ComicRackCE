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
SetupIconFile=ComicRack\Icons\icon1.ico
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
Source: "ComicRack\bin\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\Changes.txt"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\ComicRack.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\ComicRack.exe.config"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\ComicRack.ini"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\DefaultLists.txt"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\License.txt"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\NewsTemplate.html"; DestDir: "{app}"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\ReadMe.txt"; DestDir: "{app}"; Flags: ignoreversion isreadme; Components: app
Source: "ComicRack\bin\Release\Languages\*"; DestDir: "{app}\Languages"; Flags: ignoreversion; Components: languages
Source: "ComicRack\bin\Release\Resources\*"; DestDir: "{app}\Resources"; Flags: ignoreversion; Components: app
Source: "ComicRack\bin\Release\Resources\Icons\*"; DestDir: "{app}\Resources\Icons"; Flags: ignoreversion; Components: additional
Source: "ComicRack\bin\Release\Resources\Textures\*"; DestDir: "{app}\Resources\Textures"; Flags: ignoreversion recursesubdirs; Components: additional
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
; .cbr, .cbz, .crplugin
Root: HKA; Subkey: "Software\Classes\.cbz\OpenWithProgids"; ValueType: string; ValueName: "eComic.cbz"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\eComic.cbz"; ValueType: string; ValueName: ""; ValueData: "Comic Book File (Zip)"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\eComic.cbz\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\eComic.cbz\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}";                Components: start_menu
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}";     Components: start_menu
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; Components: start_menu
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}";          Components: desktop

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

