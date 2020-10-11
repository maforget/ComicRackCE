using System;

namespace cYo.Common.Win32
{
	[Flags]
	public enum UserCredentialsDialogFlags
	{
		Default = 0x600C0,
		None = 0x0,
		IncorrectPassword = 0x1,
		DoNotPersist = 0x2,
		RequestAdministrator = 0x4,
		ExcludesCertificates = 0x8,
		RequireCertificate = 0x10,
		ShowSaveCheckbox = 0x40,
		AlwaysShowUI = 0x80,
		RequireSmartCard = 0x100,
		PasswordOnlyOk = 0x200,
		ValidateUsername = 0x400,
		CompleteUserName = 0x800,
		Persist = 0x1000,
		ServerCredential = 0x4000,
		ExpectConfirmation = 0x20000,
		GenericCredentials = 0x40000,
		UsernameTargetCredentials = 0x80000,
		KeepUsername = 0x100000
	}
}
