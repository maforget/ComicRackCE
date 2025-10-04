using System;
using System.Runtime.CompilerServices;


namespace cYo.Common.Windows
{
    /// <summary>
    /// source: <a href="https://github.com/dotnet/winforms">dotnet/winforms</a>. (.NET Foundation, MIT license)<br/>
    /// <c>src/System.Private.Windows.Core/src/System/Private/Windows/OsVersion.cs</c>
    /// </summary>
    public static class OsVersionEx
    {
        /// <summary>
        ///  Is Windows 10 first release or later. (Threshold 1, build 10240, version 1507)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows10.0.10240")]
        public static bool IsWindows10_1507OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 10, build: 10240);

        /// <summary>
        ///  Is Windows 10 Anniversary Update or later. (Redstone 1, build 14393, version 1607)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows10.0.14393")]
        public static bool IsWindows10_1607OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 10, build: 14393);

        /// <summary>
        ///  Is Windows 10 Creators Update or later. (Redstone 2, build 15063, version 1703)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows10.0.15063")]
        public static bool IsWindows10_1703OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 10, build: 15063);

        /// <summary>
        ///  Is Windows 10 Creators Update or later. (Redstone 3, build 16299, version 1709)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows10.0.16299")]
        public static bool IsWindows10_1709OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 10, build: 16299);

        /// <summary>
        ///  Is Windows 10 Creators Update or later. (Redstone 4, build 17134, version 1803)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows10.0.17134")]
        public static bool IsWindows10_18030rGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 10, build: 17134);

        /// <summary>
        ///  Is this Windows 11 public preview or later?
        ///  The underlying API does not read supportedOs from the manifest, it returns the actual version.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows11")]
        public static bool IsWindows11_OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 10, build: 22000);

        /// <summary>
        ///  Is this Windows 11 version 22H2 or greater?
        ///  The underlying API does not read supportedOs from the manifest, it returns the actual version.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows11.0.22621")]
        public static bool IsWindows11_22H2OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 10, build: 22621);

        /// <summary>
        ///  Is Windows 8.1 or later.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows8.1")]
        public static bool IsWindows8_1OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 6, minor: 3);

        /// <summary>
        ///  Is Windows 8 or later.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[SupportedOSPlatformGuard("windows8")]
        public static bool IsWindows8OrGreater() => OperatingSystem.IsWindowsVersionAtLeast(major: 6, minor: 2);
    }

    /// <summary>
    /// source: <a href="https://github.com/dotnet/runtime">dotnet/runtime</a>. (.NET Foundation, MIT license)<br/>
    /// <c>src/libraries/System.Private.CoreLib/src/System/OperatingSystem.cs</c>
    /// </summary>
    /// <remarks>
    /// Heavily reduced as we we're only running on Windows, and just need for the convenient OsVersionEx class.
    /// </remarks>
    internal sealed class OperatingSystem
    {
        public static bool IsWindows() => true;

        /// <summary>
        /// Check for the Windows version (returned by 'RtlGetVersion') with a >= version comparison. Used to guard APIs that were added in the given Windows release.
        /// </summary>
        public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
            => IsWindows() && IsOSVersionAtLeast(major, minor, build, revision);

        private static bool IsOSVersionAtLeast(int major, int minor, int build, int revision)
        {
            Version current = Environment.OSVersion.Version;

            if (current.Major != major)
            {
                return current.Major > major;
            }
            if (current.Minor != minor)
            {
                return current.Minor > minor;
            }
            // Unspecified build component is to be treated as zero
            int currentBuild = current.Build < 0 ? 0 : current.Build;
            build = build < 0 ? 0 : build;
            if (currentBuild != build)
            {
                return currentBuild > build;
            }

            // Unspecified revision component is to be treated as zero
            int currentRevision = current.Revision < 0 ? 0 : current.Revision;
            revision = revision < 0 ? 0 : revision;

            return currentRevision >= revision;
        }
    }
}
