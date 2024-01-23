; NSIS script NSIS-2
; Install

SetCompressor lzma
SetCompressorDictSize 8

; --------------------
; HEADER SIZE: 41699
; START HEADER SIZE: 300
; MAX STRING LENGTH: 1024
; STRING CHARS: 9309

OutFile [NSIS].exe
!include WinMessages.nsh

ShowInstDetails show
InstallDirRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\App Paths\ComicRack.exe" ""
LicenseBkColor /windows


; --------------------
; LANG TABLES: 1
; LANG STRINGS: 88

Name "ComicRack v0.9.178"
BrandingText "Nullsoft Install System v2.46"

; LANG: 1033
LangString LSTR_0 1033 "Nullsoft Install System v2.46"
LangString LSTR_1 1033 "$(LSTR_2) Setup"
LangString LSTR_2 1033 "ComicRack v0.9.178"
LangString LSTR_3 1033 "Space available: "
LangString LSTR_4 1033 "Space required: "
LangString LSTR_5 1033 "Can't write: "
LangString LSTR_8 1033 "Could not find symbol: "
LangString LSTR_9 1033 "Could not load: "
LangString LSTR_10 1033 "Create folder: "
LangString LSTR_11 1033 "Create shortcut: "
LangString LSTR_12 1033 "Created uninstaller: "
LangString LSTR_13 1033 "Delete file: "
LangString LSTR_14 1033 "Delete on reboot: "
LangString LSTR_15 1033 "Error creating shortcut: "
LangString LSTR_16 1033 "Error creating: "
LangString LSTR_17 1033 "Error decompressing data! Corrupted installer?"
LangString LSTR_19 1033 "ExecShell: "
LangString LSTR_20 1033 "Execute: "
LangString LSTR_21 1033 "Extract: "
LangString LSTR_22 1033 "Extract: error writing to file "
LangString LSTR_23 1033 "Installer corrupted: invalid opcode"
LangString LSTR_24 1033 "No OLE for: "
LangString LSTR_25 1033 "Output folder: "
LangString LSTR_26 1033 "Remove folder: "
LangString LSTR_29 1033 "Skipped: "
LangString LSTR_30 1033 "Copy Details To Clipboard"
LangString LSTR_32 1033 B
LangString LSTR_33 1033 K
LangString LSTR_34 1033 M
LangString LSTR_35 1033 G
LangString LSTR_36 1033 "Error opening file for writing: $\r$\n$\r$\n$0$\r$\n$\r$\nClick Abort to stop the installation,$\r$\nRetry to try again, or$\r$\nIgnore to skip this file."
LangString LSTR_37 1033 0
LangString LSTR_38 1033 "Welcome to the $(LSTR_87) Setup Wizard"
LangString LSTR_39 1033 "MS Shell Dlg"
LangString LSTR_40 1033 "This wizard will guide you through the installation of $(LSTR_87).$\r$\n$\r$\nIt is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer.$\r$\n$\r$\n$_CLICK"
LangString LSTR_41 1033 "If you accept the terms of the agreement, click I Agree to continue. You must accept the agreement to install $(LSTR_87)."
LangString LSTR_42 1033 "License Agreement"
LangString LSTR_43 1033 "Please review the license terms before installing $(LSTR_87)."
LangString LSTR_44 1033 "Press Page Down to see the rest of the agreement."
LangString LSTR_45 1033 "Choose Components"
LangString LSTR_46 1033 "Choose which features of $(LSTR_87) you want to install."
LangString LSTR_47 1033 Description
LangString LSTR_48 1033 "Position your mouse over a component to see its description."
LangString LSTR_49 1033 "Choose Install Location"
LangString LSTR_50 1033 "Choose the folder in which to install $(LSTR_87)."
LangString LSTR_51 1033 Installing
LangString LSTR_52 1033 "Please wait while $(LSTR_87) is being installed."
LangString LSTR_53 1033 "Installation Complete"
LangString LSTR_54 1033 "Setup was completed successfully."
LangString LSTR_55 1033 "Installation Aborted"
LangString LSTR_56 1033 "Setup was not completed successfully."
LangString LSTR_57 1033 &Finish
LangString LSTR_58 1033 "Completing the $(LSTR_87) Setup Wizard"
LangString LSTR_59 1033 "Your computer must be restarted in order to complete the installation of $(LSTR_87). Do you want to reboot now?"
LangString LSTR_60 1033 "Reboot now"
LangString LSTR_61 1033 "I want to manually reboot later"
LangString LSTR_62 1033 "$(LSTR_87) has been installed on your computer.$\r$\n$\r$\nClick Finish to close this wizard."
LangString LSTR_63 1033 "&Run $(LSTR_87)"
LangString LSTR_64 1033 "&Show Readme"
LangString LSTR_65 1033 8
LangString LSTR_66 1033 "Are you sure you want to quit $(LSTR_2) Setup?"
LangString LSTR_67 1033 Custom
LangString LSTR_68 1033 Cancel
LangString LSTR_69 1033 "< &Back"
LangString LSTR_70 1033 "&Next >"
LangString LSTR_71 1033 "Click Next to continue."
LangString LSTR_72 1033 "I &Agree"
LangString LSTR_73 1033 "Check the components you want to install and uncheck the components you don't want to install. $_CLICK"
LangString LSTR_74 1033 "Select the type of install:"
LangString LSTR_75 1033 "Or, select the optional components you wish to install:"
LangString LSTR_76 1033 "Select components to install:"
LangString LSTR_77 1033 "Setup will install $(LSTR_87) in the following folder. To install in a different folder, click Browse and select another folder. $_CLICK"
LangString LSTR_78 1033 "Destination Folder"
LangString LSTR_79 1033 B&rowse...
LangString LSTR_80 1033 "Select the folder to install $(LSTR_87) in:"
LangString LSTR_81 1033 &Install
LangString LSTR_82 1033 "Click Install to start the installation."
LangString LSTR_83 1033 "Show &details"
LangString LSTR_84 1033 Completed
LangString LSTR_85 1033 " "
LangString LSTR_86 1033 &Close
LangString LSTR_87 1033 "ComicRack v0.9.178"


; --------------------
; VARIABLES: 60

Var _0_
Var _1_
Var _2_
Var _3_
Var _4_
Var _5_
Var _6_
Var _7_
Var _8_
Var _9_
Var _10_
Var _11_
Var _12_
Var _13_
Var _14_
Var _15_
Var _16_
Var _17_
Var _18_
Var _19_
Var _20_
Var _21_
Var _22_
Var _23_
Var _24_
Var _25_
Var _26_
Var _27_
Var _28_
Var _29_
Var _30_
Var _31_
Var _32_
Var _33_
Var _34_
Var _35_
Var _36_
Var _37_
Var _38_
Var _39_
Var _40_
Var _41_
Var _42_
Var _43_
Var _44_
Var _45_
Var _46_
Var _47_
Var _48_
Var _49_
Var _50_
Var _51_
Var _52_
Var _53_
Var _54_
Var _55_
Var _56_
Var _57_
Var _58_
Var _59_


InstType $(LSTR_67)    ;  Custom
InstallDir $PROGRAMFILES\ComicRack
; install_directory_auto_append = ComicRack
; wininit = $WINDIR\wininit.ini


; --------------------
; PAGES: 7

; Page 0
Page custom func_55 func_170 /ENABLECANCEL

; Page 1
Page license func_171 func_174 func_180 /ENABLECANCEL
  LicenseText $(LSTR_41) $(LSTR_72)    ;  "If you accept the terms of the agreement, click I Agree to continue. You must accept the agreement to install $(LSTR_87)." "I &Agree" "ComicRack v0.9.178"
  LicenseData [LICENSE].txt

; Page 2
Page components func_181 func_184 func_198 /ENABLECANCEL
  ComponentsText $(LSTR_73) $(LSTR_74) $(LSTR_75)    ;  "Check the components you want to install and uncheck the components you don't want to install. $_CLICK" "Select the type of install:" "Or, select the optional components you wish to install:"

; Page 3
Page directory func_199 func_202 func_210 /ENABLECANCEL
  DirText $(LSTR_77) $(LSTR_78) $(LSTR_79) $(LSTR_80)    ;  "Setup will install $(LSTR_87) in the following folder. To install in a different folder, click Browse and select another folder. $_CLICK" "Destination Folder" B&rowse... "Select the folder to install $(LSTR_87) in:" "ComicRack v0.9.178" "ComicRack v0.9.178"
  DirVar $CMDLINE

; Page 4
Page instfiles func_211 func_214 func_220
  CompletedText $(LSTR_84)    ;  Completed
  DetailsButtonText $(LSTR_83)    ;  "Show &details"

/*
; Page 5
Page COMPLETED
*/

; Page 6
Page custom func_234 func_451


; --------------------
; SECTIONS: 8
; COMMANDS: 816

Function func_0
  ReadRegDWORD $_12_ HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" Release
  IntCmp $_12_ 378389 label_2 label_4 label_3
label_2:
  Goto label_29
label_3:
  Goto label_29
label_4:
  Goto label_5
label_5:
  IfSilent label_6 label_8
label_6:
  StrCpy $_13_ "/q /norestart"
  Goto label_10
label_8:
  StrCpy $_13_ "/showrmui /passive /norestart"
  Goto label_10
label_10:
  IfFileExists $EXEPATH\components\dotNET45Full.exe label_11 label_13
label_11:
  ExecWait "$\"$EXEPATH\components\dotNET45Full.exe$\" $_13_" $_14_
  Goto label_25
label_13:
  NSISdl::download http://go.microsoft.com/fwlink/?LinkId=225704 $TEMP\dotNET45Web.exe
    ; Call Initialize_____Plugins
    ; SetOverwrite off
    ; File $PLUGINSDIR\NSISdl.dll
    ; SetDetailsPrint lastused
    ; Push $TEMP\dotNET45Web.exe
    ; Push http://go.microsoft.com/fwlink/?LinkId=225704
    ; CallInstDLL $PLUGINSDIR\NSISdl.dll download
  Pop $_15_
  StrCmp $_15_ success label_21 label_23
label_21:
  ExecWait "$\"$TEMP\dotNET45Web.exe$\" $_13_" $_14_
  Goto label_25
label_23:
  MessageBox MB_OK|MB_ICONEXCLAMATION "Unable to download .NET Framework.  ComicRack will be installed, but will not function without the Framework!"
  Goto label_30
label_25:
  IntCmp $_14_ 1641 0 label_27 label_27
  Goto label_28
label_27:
  IntCmp $_14_ 3010 0 label_29 label_29
label_28:
  SetRebootFlag true
label_29:
  Goto label_30
label_30:
FunctionEnd


Function func_31
  InitPluginsDir
    ; Call Initialize_____Plugins
    ; SetDetailsPrint lastused
  SetOverwrite on
  File $PLUGINSDIR\modern-wizard.bmp
FunctionEnd


Function func_35
  LockWindow on
  ShowWindow $_6_ ${SW_HIDE}
  ShowWindow $_5_ ${SW_HIDE}
  ShowWindow $_0_ ${SW_HIDE}
  ShowWindow $_2_ ${SW_HIDE}
  ShowWindow $_4_ ${SW_HIDE}
  ShowWindow $_7_ ${SW_HIDE}
  ShowWindow $_8_ ${SW_SHOWNORMAL}
  LockWindow off
FunctionEnd


Function func_45
  LockWindow on
  ShowWindow $_6_ ${SW_SHOWNORMAL}
  ShowWindow $_5_ ${SW_SHOWNORMAL}
  ShowWindow $_0_ ${SW_SHOWNORMAL}
  ShowWindow $_2_ ${SW_SHOWNORMAL}
  ShowWindow $_4_ ${SW_SHOWNORMAL}
  ShowWindow $_7_ ${SW_SHOWNORMAL}
  ShowWindow $_8_ ${SW_HIDE}
  LockWindow off
FunctionEnd


Function func_55    ; Page 0, Pre
  nsDialogs::Create 1044
    ; Call Initialize_____Plugins
    ; SetOverwrite off
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push 1044
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll Create
  Pop $_16_
  nsDialogs::SetRTL $(LSTR_37)    ;  0
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_37)    ;  0
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll SetRTL
  SetCtlColors $_16_ "" 0xFFFFFF
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x0000000E|0x00000100 0 0u 0u 109u 193u ""
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push ""
    ; Push 193u
    ; Push 109u
    ; Push 0u
    ; Push 0u
    ; Push 0
    ; Push 0x40000000|0x10000000|0x04000000|0x0000000E|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_17_
  Push $0
  Push $1
  Push $2
  Push $R0
  StrCpy $R0 $_17_
  StrCpy $1 ""
  StrCpy $2 ""
  System::Call "*(i, i, i, i) i.s"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push "*(i, i, i, i) i.s"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $0
  IntCmp $0 0 label_111
  System::Call "user32::GetClientRect(iR0, ir0)"
    ; Call Initialize_____Plugins
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push "user32::GetClientRect(iR0, ir0)"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Call "*$0(i, i, i .s, i .s)"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push "*$0(i, i, i .s, i .s)"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Free $0
    ; Call Initialize_____Plugins
    ; AllowSkipFiles on
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push $0
    ; CallInstDLL $PLUGINSDIR\System.dll Free
  Pop $1
  Pop $2
label_111:
  System::Call "user32::LoadImage(i0, ts, i 0, ir1, ir2, i0x0010) i.s" $PLUGINSDIR\modern-wizard.bmp
    ; Call Initialize_____Plugins
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push $PLUGINSDIR\modern-wizard.bmp
    ; Push "user32::LoadImage(i0, ts, i 0, ir1, ir2, i0x0010) i.s"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $0
  SendMessage $R0 0x0172 0 $0
  Pop $R0
  Pop $2
  Pop $1
  Exch $0
    ; Push $0
    ; Exch
    ; Pop $0
  Pop $_18_
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x00000100 0x00000020 120u 10u 195u 28u $(LSTR_38)    ;  "Welcome to the $(LSTR_87) Setup Wizard" "ComicRack v0.9.178"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_38)    ;  "Welcome to the $(LSTR_87) Setup Wizard" "ComicRack v0.9.178"
    ; Push 28u
    ; Push 195u
    ; Push 10u
    ; Push 120u
    ; Push 0x00000020
    ; Push 0x40000000|0x10000000|0x04000000|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_19_
  SetCtlColors $_19_ "" 0xFFFFFF
  CreateFont $_20_ $(LSTR_39) 12 700    ;  "MS Shell Dlg"
  SendMessage $_19_ ${WM_SETFONT} $_20_ 0
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x00000100 0x00000020 120u 45u 195u 130u $(LSTR_40)    ;  "This wizard will guide you through the installation of $(LSTR_87).$\r$\n$\r$\nIt is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer.$\r$\n$\r$\n$_CLICK" "ComicRack v0.9.178"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_40)    ;  "This wizard will guide you through the installation of $(LSTR_87).$\r$\n$\r$\nIt is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer.$\r$\n$\r$\n$_CLICK" "ComicRack v0.9.178"
    ; Push 130u
    ; Push 195u
    ; Push 45u
    ; Push 120u
    ; Push 0x00000020
    ; Push 0x40000000|0x10000000|0x04000000|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_21_
  SetCtlColors $_21_ "" 0xFFFFFF
  Call func_35
  nsDialogs::Show
    ; Call Initialize_____Plugins
    ; AllowSkipFiles on
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll Show
  Call func_45
  IntCmp $_18_ 0 label_169
  System::Call gdi32::DeleteObject(is) $_18_
    ; Call Initialize_____Plugins
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push $_18_
    ; Push gdi32::DeleteObject(is)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
label_169:
FunctionEnd


Function func_170    ; Page 0, Leave
FunctionEnd


Function func_171    ; Page 1, Pre
  SendMessage $_0_ ${WM_SETTEXT} 0 STR:$(LSTR_42)    ;  "License Agreement"
  SendMessage $_2_ ${WM_SETTEXT} 0 STR:$(LSTR_43)    ;  "Please review the license terms before installing $(LSTR_87)." "ComicRack v0.9.178"
FunctionEnd


Function func_174    ; Page 1, Show
  FindWindow $_22_ "#32770" "" $HWNDPARENT
  GetDlgItem $_23_ $_22_ 1040
  GetDlgItem $_24_ $_22_ 1006
  GetDlgItem $_25_ $_22_ 1000
  SendMessage $_23_ ${WM_SETTEXT} 0 STR:$(LSTR_44)    ;  "Press Page Down to see the rest of the agreement."
FunctionEnd


Function func_180    ; Page 1, Leave
FunctionEnd


Function func_181    ; Page 2, Pre
  SendMessage $_0_ ${WM_SETTEXT} 0 STR:$(LSTR_45)    ;  "Choose Components"
  SendMessage $_2_ ${WM_SETTEXT} 0 STR:$(LSTR_46)    ;  "Choose which features of $(LSTR_87) you want to install." "ComicRack v0.9.178"
FunctionEnd


Function func_184    ; Page 2, Show
  FindWindow $_26_ "#32770" "" $HWNDPARENT
  GetDlgItem $_27_ $_26_ 1006
  GetDlgItem $_28_ $_26_ 1021
  GetDlgItem $_29_ $_26_ 1022
  GetDlgItem $_30_ $_26_ 1017
  GetDlgItem $_31_ $_26_ 1032
  GetDlgItem $_32_ $_26_ 1042
  GetDlgItem $_34_ $_26_ 1043
  GetDlgItem $_35_ $_26_ 1023
  SendMessage $_32_ ${WM_SETTEXT} 0 STR:$(LSTR_47)    ;  Description
  EnableWindow $_34_ 0
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:$(LSTR_48)    ;  "Position your mouse over a component to see its description."
  StrCpy $_33_ $(LSTR_48)    ;  "Position your mouse over a component to see its description."
FunctionEnd


Function func_198    ; Page 2, Leave
FunctionEnd


Function func_199    ; Page 3, Pre
  SendMessage $_0_ ${WM_SETTEXT} 0 STR:$(LSTR_49)    ;  "Choose Install Location"
  SendMessage $_2_ ${WM_SETTEXT} 0 STR:$(LSTR_50)    ;  "Choose the folder in which to install $(LSTR_87)." "ComicRack v0.9.178"
FunctionEnd


Function func_202    ; Page 3, Show
  FindWindow $_36_ "#32770" "" $HWNDPARENT
  GetDlgItem $_37_ $_36_ 1006
  GetDlgItem $_38_ $_36_ 1020
  GetDlgItem $_39_ $_36_ 1019
  GetDlgItem $_40_ $_36_ 1001
  GetDlgItem $_41_ $_36_ 1023
  GetDlgItem $_42_ $_36_ 1024
FunctionEnd


Function func_210    ; Page 3, Leave
FunctionEnd


Function func_211    ; Page 4, Pre
  SendMessage $_0_ ${WM_SETTEXT} 0 STR:$(LSTR_51)    ;  Installing
  SendMessage $_2_ ${WM_SETTEXT} 0 STR:$(LSTR_52)    ;  "Please wait while $(LSTR_87) is being installed." "ComicRack v0.9.178"
FunctionEnd


Function func_214    ; Page 4, Show
  FindWindow $_43_ "#32770" "" $HWNDPARENT
  GetDlgItem $_44_ $_43_ 1006
  GetDlgItem $_45_ $_43_ 1004
  GetDlgItem $_46_ $_43_ 1027
  GetDlgItem $_47_ $_43_ 1016
FunctionEnd


Function func_220    ; Page 4, Leave
  IfAbort label_224
  SendMessage $_0_ ${WM_SETTEXT} 0 STR:$(LSTR_53)    ;  "Installation Complete"
  SendMessage $_2_ ${WM_SETTEXT} 0 STR:$(LSTR_54)    ;  "Setup was completed successfully."
  Goto label_226
label_224:
  SendMessage $_0_ ${WM_SETTEXT} 0 STR:$(LSTR_55)    ;  "Installation Aborted"
  SendMessage $_2_ ${WM_SETTEXT} 0 STR:$(LSTR_56)    ;  "Setup was not completed successfully."
label_226:
  IfAbort label_227
label_227:
FunctionEnd


Function func_228
  InitPluginsDir
    ; Call Initialize_____Plugins
    ; SetDetailsPrint lastused
  SetOverwrite on
  AllowSkipFiles on
  File $PLUGINSDIR\modern-wizard.bmp
  Call func_31
  SetAutoClose true
FunctionEnd


Function func_234    ; Page 6, Pre
  SendMessage $_9_ ${WM_SETTEXT} 0 STR:$(LSTR_57)    ;  &Finish
  nsDialogs::Create 1044
    ; Call Initialize_____Plugins
    ; SetOverwrite off
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push 1044
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll Create
  Pop $_48_
  nsDialogs::SetRTL $(LSTR_37)    ;  0
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_37)    ;  0
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll SetRTL
  SetCtlColors $_48_ "" 0xFFFFFF
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x0000000E|0x00000100 0 0u 0u 109u 193u ""
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push ""
    ; Push 193u
    ; Push 109u
    ; Push 0u
    ; Push 0u
    ; Push 0
    ; Push 0x40000000|0x10000000|0x04000000|0x0000000E|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_49_
  Push $0
  Push $1
  Push $2
  Push $R0
  StrCpy $R0 $_49_
  StrCpy $1 ""
  StrCpy $2 ""
  System::Call "*(i, i, i, i) i.s"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push "*(i, i, i, i) i.s"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $0
  IntCmp $0 0 label_291
  System::Call "user32::GetClientRect(iR0, ir0)"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push "user32::GetClientRect(iR0, ir0)"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Call "*$0(i, i, i .s, i .s)"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push "*$0(i, i, i .s, i .s)"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Free $0
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push $0
    ; CallInstDLL $PLUGINSDIR\System.dll Free
  Pop $1
  Pop $2
label_291:
  System::Call "user32::LoadImage(i0, ts, i 0, ir1, ir2, i0x0010) i.s" $PLUGINSDIR\modern-wizard.bmp
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push $PLUGINSDIR\modern-wizard.bmp
    ; Push "user32::LoadImage(i0, ts, i 0, ir1, ir2, i0x0010) i.s"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $0
  SendMessage $R0 0x0172 0 $0
  Pop $R0
  Pop $2
  Pop $1
  Exch $0
    ; Push $0
    ; Exch
    ; Pop $0
  Pop $_50_
  IfRebootFlag 0 label_372
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x00000100 0x00000020 120u 10u 195u 28u $(LSTR_58)    ;  "Completing the $(LSTR_87) Setup Wizard" "ComicRack v0.9.178"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_58)    ;  "Completing the $(LSTR_87) Setup Wizard" "ComicRack v0.9.178"
    ; Push 28u
    ; Push 195u
    ; Push 10u
    ; Push 120u
    ; Push 0x00000020
    ; Push 0x40000000|0x10000000|0x04000000|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_51_
  SetCtlColors $_51_ "" 0xFFFFFF
  CreateFont $_52_ $(LSTR_39) 12 700    ;  "MS Shell Dlg"
  SendMessage $_51_ ${WM_SETFONT} $_52_ 0
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x00000100 0x00000020 120u 45u 195u 40u $(LSTR_59)    ;  "Your computer must be restarted in order to complete the installation of $(LSTR_87). Do you want to reboot now?" "ComicRack v0.9.178"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_59)    ;  "Your computer must be restarted in order to complete the installation of $(LSTR_87). Do you want to reboot now?" "ComicRack v0.9.178"
    ; Push 40u
    ; Push 195u
    ; Push 45u
    ; Push 120u
    ; Push 0x00000020
    ; Push 0x40000000|0x10000000|0x04000000|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_53_
  SetCtlColors $_53_ "" 0xFFFFFF
  nsDialogs::CreateControl BUTTON 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000009|0x00002000 0 120u 90u 195u 10u $(LSTR_60)    ;  "Reboot now"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_60)    ;  "Reboot now"
    ; Push 10u
    ; Push 195u
    ; Push 90u
    ; Push 120u
    ; Push 0
    ; Push 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000009|0x00002000
    ; Push BUTTON
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_57_
  SetCtlColors $_57_ "" 0xFFFFFF
  nsDialogs::CreateControl BUTTON 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000009|0x00002000 0 120u 115u 195u 10u $(LSTR_61)    ;  "I want to manually reboot later"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_61)    ;  "I want to manually reboot later"
    ; Push 10u
    ; Push 195u
    ; Push 115u
    ; Push 120u
    ; Push 0
    ; Push 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000009|0x00002000
    ; Push BUTTON
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_58_
  SetCtlColors $_58_ "" 0xFFFFFF
  SendMessage $_57_ 0x00F1 1 0
  System::Call user32::SetFocus(i$_57_)
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push user32::SetFocus(i$_57_)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Goto label_437
label_372:
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x00000100 0x00000020 120u 10u 195u 28u $(LSTR_58)    ;  "Completing the $(LSTR_87) Setup Wizard" "ComicRack v0.9.178"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_58)    ;  "Completing the $(LSTR_87) Setup Wizard" "ComicRack v0.9.178"
    ; Push 28u
    ; Push 195u
    ; Push 10u
    ; Push 120u
    ; Push 0x00000020
    ; Push 0x40000000|0x10000000|0x04000000|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_51_
  SetCtlColors $_51_ "" 0xFFFFFF
  CreateFont $_52_ $(LSTR_39) 12 700    ;  "MS Shell Dlg"
  SendMessage $_51_ ${WM_SETFONT} $_52_ 0
  nsDialogs::CreateControl STATIC 0x40000000|0x10000000|0x04000000|0x00000100 0x00000020 120u 45u 195u 40u $(LSTR_62)    ;  "$(LSTR_87) has been installed on your computer.$\r$\n$\r$\nClick Finish to close this wizard." "ComicRack v0.9.178"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_62)    ;  "$(LSTR_87) has been installed on your computer.$\r$\n$\r$\nClick Finish to close this wizard." "ComicRack v0.9.178"
    ; Push 40u
    ; Push 195u
    ; Push 45u
    ; Push 120u
    ; Push 0x00000020
    ; Push 0x40000000|0x10000000|0x04000000|0x00000100
    ; Push STATIC
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_53_
  SetCtlColors $_53_ "" 0xFFFFFF
  nsDialogs::CreateControl BUTTON 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000003|0x00002000 0 120u 90u 195u 10u $(LSTR_63)    ;  "&Run $(LSTR_87)" "ComicRack v0.9.178"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_63)    ;  "&Run $(LSTR_87)" "ComicRack v0.9.178"
    ; Push 10u
    ; Push 195u
    ; Push 90u
    ; Push 120u
    ; Push 0
    ; Push 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000003|0x00002000
    ; Push BUTTON
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_55_
  SetCtlColors $_55_ "" 0xFFFFFF
  SendMessage $_55_ 0x00F1 1 0
  System::Call user32::SetFocus(i$_55_)
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push user32::SetFocus(i$_55_)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  nsDialogs::CreateControl BUTTON 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000003|0x00002000 0 120u 110u 195u 10u $(LSTR_64)    ;  "&Show Readme"
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; Push $(LSTR_64)    ;  "&Show Readme"
    ; Push 10u
    ; Push 195u
    ; Push 110u
    ; Push 120u
    ; Push 0
    ; Push 0x40000000|0x10000000|0x04000000|0x00010000|0x00000000|0x00000C00|0x00000003|0x00002000
    ; Push BUTTON
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll CreateControl
  Pop $_56_
  SetCtlColors $_56_ "" 0xFFFFFF
  SendMessage $_56_ 0x00F1 1 0
label_437:
  Call func_35
  nsDialogs::Show
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\nsDialogs.dll
    ; SetDetailsPrint lastused
    ; CallInstDLL $PLUGINSDIR\nsDialogs.dll Show
  Call func_45
  IntCmp $_50_ 0 label_450
  System::Call gdi32::DeleteObject(is) $_50_
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push $_50_
    ; Push gdi32::DeleteObject(is)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
label_450:
FunctionEnd


Function func_451    ; Page 6, Leave
  IfRebootFlag 0 label_458
  SendMessage $_57_ 0x00F0 0 0 $_54_
  IntCmp $_54_ 1 0 label_457 label_457
  Reboot
    ; Quit
  Goto label_458
label_457:
  Return

label_458:
  SendMessage $_55_ 0x00F0 0 0 $_54_
  IntCmp $_54_ 1 0 label_461 label_461
  Exec $\"$INSTDIR\ComicRack.exe$\"
label_461:
  SendMessage $_56_ 0x00F0 0 0 $_54_
  IntCmp $_54_ 1 0 label_464 label_464
  ExecShell open $INSTDIR\ReadMe.txt    ; "open $INSTDIR\ReadMe.txt"
label_464:
FunctionEnd


Function .onGUIInit
  GetDlgItem $_0_ $HWNDPARENT 1037
  CreateFont $_1_ $(LSTR_39) $(LSTR_65) 700    ;  "MS Shell Dlg" 8
  SendMessage $_0_ ${WM_SETFONT} $_1_ 0
  GetDlgItem $_2_ $HWNDPARENT 1038
  SetCtlColors $_0_ "" 0xFFFFFF
  SetCtlColors $_2_ "" 0xFFFFFF
  GetDlgItem $_3_ $HWNDPARENT 1034
  SetCtlColors $_3_ "" 0xFFFFFF
  GetDlgItem $_4_ $HWNDPARENT 1039
  SetCtlColors $_4_ "" 0xFFFFFF
  GetDlgItem $_6_ $HWNDPARENT 1028
  SetCtlColors $_6_ /BRANDING ""
  GetDlgItem $_5_ $HWNDPARENT 1256
  SetCtlColors $_5_ /BRANDING ""
  SendMessage $_5_ ${WM_SETTEXT} 0 "STR:$(LSTR_0) "    ;  "Nullsoft Install System v2.46"
  GetDlgItem $_7_ $HWNDPARENT 1035
  GetDlgItem $_8_ $HWNDPARENT 1045
  GetDlgItem $_9_ $HWNDPARENT 1
  GetDlgItem $_10_ $HWNDPARENT 2
  GetDlgItem $_11_ $HWNDPARENT 3
  Call func_228
FunctionEnd


Function .onUserAbort
  MessageBox MB_YESNO|MB_ICONEXCLAMATION $(LSTR_66) IDYES label_489    ;  "Are you sure you want to quit $(LSTR_2) Setup?" "ComicRack v0.9.178"
  Abort
label_489:
FunctionEnd


Section "ComicRack (Required)" ; Section_0
  ; AddSize 18107
  SectionIn 1 RO
  SetShellVarContext all
  Call func_0
  SetOutPath $INSTDIR
  SetOverwrite ifdiff
  AllowSkipFiles on
  File ComicRack.exe
  File ComicRack.exe.config
  File cYo.Common.dll
  File cYo.Common.Presentation.dll
  File cYo.Common.Windows.dll
  File ComicRack.Engine.dll
  File ComicRack.Engine.Display.Forms.dll
  File ComicRack.Plugins.dll
  File sharpPDF.dll
  File IronPython.dll
  File IronPython.Modules.dll
  File Microsoft.Dynamic.dll
  File Microsoft.Scripting.dll
  File Microsoft.Scripting.Metadata.dll
  File Microsoft.WindowsAPICodePack.dll
  File Microsoft.WindowsAPICodePack.Shell.dll
  File Windows7.Multitouch.dll
  File Tao.OpenGl.dll
  File Tao.Platform.Windows.dll
  File ICSharpCode.SharpZipLib.dll
  File SharpCompress.dll
  File MySqlConnector.dll
  File NewsTemplate.html
  File ComicRack.ini
  File ReadMe.txt
  File Changes.txt
  File DefaultLists.txt
  File License.txt
  SetOutPath $INSTDIR\Help
  File "Help\ComicRack Introduction.djvu"
  File "Help\ComicRack Introduction.djvu.xml"
  File "Help\ComicRack Online Manual.ini"
  File "Help\ComicRack Wiki.ini"
  File Help\readme.txt
  SetOutPath $INSTDIR\Resources
  File Resources\7z.dll
  System::Call kernel32::GetCurrentProcess()i.s
    ; Call Initialize_____Plugins
    ; SetOverwrite off
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::GetCurrentProcess()i.s
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Call kernel32::IsWow64Process(is,*i.s)
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::IsWow64Process(is,*i.s)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $_59_
  StrCmp $_59_ 0 label_543
  SetOverwrite ifdiff
  AllowSkipFiles on
  File Resources\7z64.dll
label_543:
  File Resources\7z.exe
  System::Call kernel32::GetCurrentProcess()i.s
    ; Call Initialize_____Plugins
    ; SetOverwrite off
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::GetCurrentProcess()i.s
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Call kernel32::IsWow64Process(is,*i.s)
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::IsWow64Process(is,*i.s)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $_59_
  StrCmp $_59_ 0 label_558
  SetOverwrite ifdiff
  AllowSkipFiles on
  File Resources\libwebp64.dll
  Goto label_559
label_558:
  File Resources\libwebp32.dll
label_559:
  File Resources\c44.exe
  File Resources\ddjvu.exe
  File Resources\djvm.exe
  File Resources\libdjvulibre.dll
  File Resources\libjpeg.dll
  File Resources\libtiff.dll
  File Resources\libz.dll
  SetOutPath $INSTDIR\Scripts
  File Scripts\Autonumber.py
  File Scripts\CommitProposed.py
  File Scripts\NewComics.py
  File Scripts\OtherScripts.py
  File Scripts\Package.ini
  File Scripts\Renumber.png
  File Scripts\Sample.py
  File Scripts\Sample.xml
  File Scripts\SearchAndReplace.png
  File Scripts\SearchAndReplace.py
SectionEnd


Section "Start Menu" ; Section_1
  CreateDirectory $SMPROGRAMS\ComicRack
  CreateShortCut $SMPROGRAMS\ComicRack\ComicRack.lnk $INSTDIR\ComicRack.exe
  CreateShortCut "$SMPROGRAMS\ComicRack\Release Notes.lnk" $INSTDIR\ReadMe.txt
  CreateShortCut "$SMPROGRAMS\ComicRack\Version History.lnk" $INSTDIR\Changes.txt
  CreateShortCut "$SMPROGRAMS\ComicRack\Quick Introduction.lnk" "$INSTDIR\Help\ComicRack Introduction.djvu"
  WriteINIStr $INSTDIR\ComicRack.url InternetShortcut URL http://comicrack.cyolito.com/
  CreateShortCut $SMPROGRAMS\ComicRack\Website.lnk $INSTDIR\ComicRack.url
SectionEnd


Section "Desktop Shortcut" ; Section_2
  CreateShortCut $DESKTOP\ComicRack.lnk $INSTDIR\ComicRack.exe
SectionEnd


Section "Associate eComic extensions" ; Section_3
  ReadRegStr $R0 HKCR .cbz ""
  ReadRegStr $R1 HKCR cYo.ComicRack_backup ""
  IfErrors 0 label_592
  WriteRegStr HKCR .cbz cYo.ComicRack_backup $R0
label_592:
  WriteRegStr HKCR .cbz "" cYo.ComicRack
  WriteRegStr HKCR cYo.ComicRack "" eComic
  WriteRegStr HKCR cYo.ComicRack\DefaultIcon "" $\"$INSTDIR\comicrack.exe$\",1
  WriteRegStr HKCR cYo.ComicRack\shell "" open
  WriteRegStr HKCR cYo.ComicRack\shell\open "" "Open eComic with ComicRack"
  WriteRegStr HKCR cYo.ComicRack\shell\open\command "" "$\"$INSTDIR\ComicRack.exe$\" $\"%1$\""
  ReadRegStr $R0 HKCR .cbt ""
  ReadRegStr $R1 HKCR cYo.ComicRack_backup ""
  IfErrors 0 label_602
  WriteRegStr HKCR .cbt cYo.ComicRack_backup $R0
label_602:
  WriteRegStr HKCR .cbt "" cYo.ComicRack
  WriteRegStr HKCR cYo.ComicRack "" eComic
  WriteRegStr HKCR cYo.ComicRack\DefaultIcon "" $\"$INSTDIR\comicrack.exe$\",1
  WriteRegStr HKCR cYo.ComicRack\shell "" open
  WriteRegStr HKCR cYo.ComicRack\shell\open "" "Open eComic with ComicRack"
  WriteRegStr HKCR cYo.ComicRack\shell\open\command "" "$\"$INSTDIR\ComicRack.exe$\" $\"%1$\""
  ReadRegStr $R0 HKCR .cbr ""
  ReadRegStr $R1 HKCR cYo.ComicRack_backup ""
  IfErrors 0 label_612
  WriteRegStr HKCR .cbr cYo.ComicRack_backup $R0
label_612:
  WriteRegStr HKCR .cbr "" cYo.ComicRack
  WriteRegStr HKCR cYo.ComicRack "" eComic
  WriteRegStr HKCR cYo.ComicRack\DefaultIcon "" $\"$INSTDIR\comicrack.exe$\",1
  WriteRegStr HKCR cYo.ComicRack\shell "" open
  WriteRegStr HKCR cYo.ComicRack\shell\open "" "Open eComic with ComicRack"
  WriteRegStr HKCR cYo.ComicRack\shell\open\command "" "$\"$INSTDIR\ComicRack.exe$\" $\"%1$\""
  ReadRegStr $R0 HKCR .cb7 ""
  ReadRegStr $R1 HKCR cYo.ComicRack_backup ""
  IfErrors 0 label_622
  WriteRegStr HKCR .cb7 cYo.ComicRack_backup $R0
label_622:
  WriteRegStr HKCR .cb7 "" cYo.ComicRack
  WriteRegStr HKCR cYo.ComicRack "" eComic
  WriteRegStr HKCR cYo.ComicRack\DefaultIcon "" $\"$INSTDIR\comicrack.exe$\",1
  WriteRegStr HKCR cYo.ComicRack\shell "" open
  WriteRegStr HKCR cYo.ComicRack\shell\open "" "Open eComic with ComicRack"
  WriteRegStr HKCR cYo.ComicRack\shell\open\command "" "$\"$INSTDIR\ComicRack.exe$\" $\"%1$\""
  ReadRegStr $R0 HKCR .cbw ""
  ReadRegStr $R1 HKCR cYo.ComicRack_backup ""
  IfErrors 0 label_632
  WriteRegStr HKCR .cbw cYo.ComicRack_backup $R0
label_632:
  WriteRegStr HKCR .cbw "" cYo.ComicRack
  WriteRegStr HKCR cYo.ComicRack "" eComic
  WriteRegStr HKCR cYo.ComicRack\DefaultIcon "" $\"$INSTDIR\comicrack.exe$\",1
  WriteRegStr HKCR cYo.ComicRack\shell "" open
  WriteRegStr HKCR cYo.ComicRack\shell\open "" "Open eComic with ComicRack"
  WriteRegStr HKCR cYo.ComicRack\shell\open\command "" "$\"$INSTDIR\ComicRack.exe$\" $\"%1$\""
  ReadRegStr $R0 HKCR .cbl ""
  ReadRegStr $R1 HKCR cYo.ComicList_backup ""
  IfErrors 0 label_642
  WriteRegStr HKCR .cbl cYo.ComicList_backup $R0
label_642:
  WriteRegStr HKCR .cbl "" cYo.ComicList
  WriteRegStr HKCR cYo.ComicList "" "eComic List"
  WriteRegStr HKCR cYo.ComicList\DefaultIcon "" $\"$INSTDIR\comicrack.exe$\",2
  WriteRegStr HKCR cYo.ComicList\shell "" open
  WriteRegStr HKCR cYo.ComicList\shell\open "" "Import eComic List into ComicRack"
  WriteRegStr HKCR cYo.ComicList\shell\open\command "" "$\"$INSTDIR\ComicRack.exe$\" -il $\"%1$\""
  ReadRegStr $R0 HKCR .crplugin ""
  ReadRegStr $R1 HKCR cYo.ComicRackPlugin_backup ""
  IfErrors 0 label_652
  WriteRegStr HKCR .crplugin cYo.ComicRackPlugin_backup $R0
label_652:
  WriteRegStr HKCR .crplugin "" cYo.ComicRackPlugin
  WriteRegStr HKCR cYo.ComicRackPlugin "" "ComicRack Plugin"
  WriteRegStr HKCR cYo.ComicRackPlugin\DefaultIcon "" $\"$INSTDIR\comicrack.exe$\",3
  WriteRegStr HKCR cYo.ComicRackPlugin\shell "" open
  WriteRegStr HKCR cYo.ComicRackPlugin\shell\open "" "Install Plugin into ComicRack"
  WriteRegStr HKCR cYo.ComicRackPlugin\shell\open\command "" "$\"$INSTDIR\ComicRack.exe$\" -ip $\"%1$\""
  System::Call "shell32::SHChangeNotify(i,i,i,i) (0x08000000, 0x1000, 0, 0)"
    ; Call Initialize_____Plugins
    ; SetOverwrite off
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push "shell32::SHChangeNotify(i,i,i,i) (0x08000000, 0x1000, 0, 0)"
    ; CallInstDLL $PLUGINSDIR\System.dll Call
SectionEnd


Section "Language Packs" ; Section_4
  ; AddSize 1206
  RMDir /r $INSTDIR\Languages
  SetOutPath $INSTDIR\Languages
  SetOverwrite ifdiff
  AllowSkipFiles on
  File Languages\cs-CZ.zip
  File Languages\de.zip
  File Languages\el-GR.zip
  File Languages\es.zip
  File Languages\fi.zip
  File Languages\fr.zip
  File Languages\hr.zip
  File Languages\hu.zip
  File Languages\it.zip
  File Languages\ja.zip
  File Languages\nl-BE.zip
  File Languages\pl.zip
  File Languages\pt-BR.zip
  File Languages\ru.zip
  File Languages\sk-SK.zip
  File Languages\tr.zip
  File Languages\zh-CN.zip
  File Languages\zh-Hans.zip
  File Languages\zh.zip
SectionEnd


Section "Additional images, icons and textures" ; Section_5
  ; AddSize 3282
  SetOutPath $INSTDIR\Resources\Textures\Backgrounds
  File "Resources\Textures\Backgrounds\Black [S].jpg"
  File Resources\Textures\Backgrounds\BrickWall.jpg
  File Resources\Textures\Backgrounds\BrushedMetal.jpg
  File Resources\Textures\Backgrounds\BrushedMetal2.jpg
  File Resources\Textures\Backgrounds\Ceramic.jpg
  File Resources\Textures\Backgrounds\Ceramic2.jpg
  File Resources\Textures\Backgrounds\ChalkBoard.jpg
  File Resources\Textures\Backgrounds\Circles.jpg
  File Resources\Textures\Backgrounds\Glass.jpg
  File Resources\Textures\Backgrounds\Grass.jpg
  File Resources\Textures\Backgrounds\LightWood.jpg
  File Resources\Textures\Backgrounds\OrangeMetal.jpg
  File Resources\Textures\Backgrounds\PlankWood.jpg
  File Resources\Textures\Backgrounds\Sketch.jpg
  SetOutPath $INSTDIR\Resources\Textures\Papers
  File Resources\Textures\Papers\Checkered.jpg
  File Resources\Textures\Papers\WhitePaper.jpg
  File Resources\Textures\Papers\WhitePaper2.jpg
  File Resources\Textures\Papers\WhitePaper3.jpg
  SetOutPath $INSTDIR\Resources\Icons
  File Resources\Icons\AgeRatings.zip
  File Resources\Icons\AgeRatings_Australia.zip
  File Resources\Icons\Formats.zip
  File Resources\Icons\Publishers.zip
  File Resources\Icons\Special.zip
SectionEnd


Section "Optimize ComicRack" ; Section_6
  DetailPrint "Optimizing ComicRack for your system..."
  System::Call kernel32::GetCurrentProcess()i.s
    ; Call Initialize_____Plugins
    ; SetOverwrite off
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::GetCurrentProcess()i.s
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Call kernel32::IsWow64Process(is,*i.s)
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::IsWow64Process(is,*i.s)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $_59_
  StrCmp $_59_ 0 label_732
  ExecDos::exec "$\"$WINDIR\Microsoft.NET\Framework64\v4.0.30319\ngen.exe$\" install $\"$INSTDIR\ComicRack.exe$\" /silent"
    ; Call Initialize_____Plugins
    ; AllowSkipFiles on
    ; File $PLUGINSDIR\ExecDos.dll
    ; SetDetailsPrint lastused
    ; Push "$\"$WINDIR\Microsoft.NET\Framework64\v4.0.30319\ngen.exe$\" install $\"$INSTDIR\ComicRack.exe$\" /silent"
    ; CallInstDLL $PLUGINSDIR\ExecDos.dll exec
  Goto label_737
label_732:
  ExecDos::exec "$\"$WINDIR\Microsoft.NET\Framework\v4.0.30319\ngen.exe$\" install $\"$INSTDIR\ComicRack.exe$\" /silent"
    ; Call Initialize_____Plugins
    ; AllowSkipFiles off
    ; File $PLUGINSDIR\ExecDos.dll
    ; SetDetailsPrint lastused
    ; Push "$\"$WINDIR\Microsoft.NET\Framework\v4.0.30319\ngen.exe$\" install $\"$INSTDIR\ComicRack.exe$\" /silent"
    ; CallInstDLL $PLUGINSDIR\ExecDos.dll exec
label_737:
SectionEnd


Section ; Section_7
  WriteUninstaller $INSTDIR\uninst.exe ;  $INSTDIR\$INSTDIR\uninst.exe
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\App Paths\ComicRack.exe" "" $INSTDIR\ComicRack.exe
  WriteRegStr HKLM Software\Microsoft\Windows\CurrentVersion\Uninstall\ComicRack DisplayName $(LSTR_2)    ;  "ComicRack v0.9.178"
  WriteRegStr HKLM Software\Microsoft\Windows\CurrentVersion\Uninstall\ComicRack UninstallString $INSTDIR\uninst.exe
  WriteRegStr HKLM Software\Microsoft\Windows\CurrentVersion\Uninstall\ComicRack DisplayIcon $INSTDIR\ComicRack.exe
  WriteRegStr HKLM Software\Microsoft\Windows\CurrentVersion\Uninstall\ComicRack DisplayVersion v0.9.178
  WriteRegStr HKLM Software\Microsoft\Windows\CurrentVersion\Uninstall\ComicRack URLInfoAbout http://comicrack.cyolito.com/
  WriteRegStr HKLM Software\Microsoft\Windows\CurrentVersion\Uninstall\ComicRack Publisher "cYo Soft"
SectionEnd


Function .onMouseOverSection
  StrCmp $0 -1 0 label_752
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 0
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:$_33_
  Goto label_786
label_752:
  StrCmp $0 0 0 label_757
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 1
  SendMessage $_34_ ${WM_SETTEXT} 0 "STR:The ComicRack Application with all needed components."
  Goto label_786
label_757:
  StrCmp $0 1 0 label_762
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 1
  SendMessage $_34_ ${WM_SETTEXT} 0 "STR:Add an entry to the Windows Start Menu."
  Goto label_786
label_762:
  StrCmp $0 2 0 label_767
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 1
  SendMessage $_34_ ${WM_SETTEXT} 0 "STR:Add a Desktop Shortcut."
  Goto label_786
label_767:
  StrCmp $0 3 0 label_772
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 1
  SendMessage $_34_ ${WM_SETTEXT} 0 "STR:Associate common eComic file extensions with ComicRack."
  Goto label_786
label_772:
  StrCmp $0 4 0 label_777
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 1
  SendMessage $_34_ ${WM_SETTEXT} 0 "STR:Non English User Interface translations."
  Goto label_786
label_777:
  StrCmp $0 5 0 label_782
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 1
  SendMessage $_34_ ${WM_SETTEXT} 0 "STR:Additional images, icons and textures"
  Goto label_786
label_782:
  StrCmp $0 6 0 label_786
  SendMessage $_34_ ${WM_SETTEXT} 0 STR:
  EnableWindow $_34_ 1
  SendMessage $_34_ ${WM_SETTEXT} 0 "STR:Optimizes ComicRack for faster execution on your system."
label_786:
FunctionEnd


Function .onInit
  System::Call kernel32::GetCurrentProcess()i.s
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::GetCurrentProcess()i.s
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  System::Call kernel32::IsWow64Process(is,*i.s)
    ; Call Initialize_____Plugins
    ; File $PLUGINSDIR\System.dll
    ; SetDetailsPrint lastused
    ; Push kernel32::IsWow64Process(is,*i.s)
    ; CallInstDLL $PLUGINSDIR\System.dll Call
  Pop $_59_
  StrCmp $_59_ 0 label_801
  SetRegView 64
  StrCpy $INSTDIR $PROGRAMFILES64\ComicRack
label_801:
FunctionEnd


/*
Function Initialize_____Plugins
  SetDetailsPrint none
  StrCmp $PLUGINSDIR "" 0 label_812
  Push $0
  SetErrors
  GetTempFileName $0
  Delete $0
  CreateDirectory $0
  IfErrors label_813
  StrCpy $PLUGINSDIR $0
  Pop $0
label_812:
  Return

label_813:
  MessageBox MB_OK|MB_ICONSTOP "Error! Can't initialize plug-ins directory. Please try again later." /SD IDOK
  Quit
FunctionEnd
*/



; --------------------
; UNREFERENCED STRINGS:

/*
38 CommonFilesDir
53 "$PROGRAMFILES\Common Files"
70 $COMMONFILES
*/
