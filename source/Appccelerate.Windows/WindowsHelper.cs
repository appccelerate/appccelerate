//-------------------------------------------------------------------------------
// <copyright file="WindowsHelper.cs" company="Appccelerate">
//   Copyright (c) 2008-2013
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Appccelerate.Windows
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides functionality to interact with Windows (OS).
    /// </summary>
    public static class WindowsHelper
    {
        /// <summary>
        /// Flags to tell <see cref="ExitSystem(EWX_ENUM)"/> how to Exit the system.
        /// </summary>
        [Flags]
        public enum EWX_ENUM
        {
            /// <summary>
            /// Shuts down all processes running in the logon session of the process that called the ExitWindowsEx function. 
            /// Then it logs the user off. This flag can be used only by processes running in an interactive user's logon session.
            /// </summary>
            EWX_LOGOFF = 0x00000000,

            /// <summary>
            /// Shuts down the system to a point at which it is safe to turn off the power. All file buffers have been flushed to disk, and all running processes have stopped. 
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// Specifying this flag will not turn off the power even if the system supports the power-off feature. You must specify EWX_POWEROFF to do this.
            ///    Windows XP SP1:  If the system supports the power-off feature, specifying this flag turns off the power.
            /// </summary>
            EWX_SHUTDOWN = 0x00000001,

            /// <summary>
            /// Shuts down the system and then restarts the system. 
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// </summary>
            EWX_REBOOT = 0x00000002,

            /// <summary>
            /// Shuts down the system and turns off the power. The system must support the power-off feature. 
            /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
            /// </summary>
            EWX_POWEROFF = 0x00000008,

            /// <summary>
            /// Forces processes to terminate. When this flag is set, the system does not send the WM_QUERYENDSESSION and WM_ENDSESSION messages. This can cause the applications to lose data. Therefore, you should only use this flag in an emergency.
            ///    Windows XP:  If the computer is locked and this flag is not specified, the shutdown process will fail.
            /// </summary>
            EWX_FORCE = 0x00000004,

            /// <summary>
            /// Forces processes to terminate if they do not respond to the WM_QUERYENDSESSION or WM_ENDSESSION message within the 
            /// timeout interval. For more information, see the Remarks. This flag is ignored if EWX_FORCE is used.
            ///    Windows NT and Windows Me/98/95:  This value is not supported.
            /// </summary>
            EWX_FORCEIFHUNG = 0x00000010,

            /// <summary>
            /// EWX_REBOOT | EWX_FORCE
            /// </summary>
            EWX_FORCEREBOOT = EWX_REBOOT | EWX_FORCE,

            /// <summary>
            /// EWX_REBOOT | EWX_FORCEIFHUNG
            /// </summary>
            EWX_FORCEIFHUNGREBOOT = EWX_REBOOT | EWX_FORCEIFHUNG,

            /// <summary>
            /// EWX_SHUTDOWN | EWX_FORCE
            /// </summary>
            EWX_FORCESHUTDOWN = EWX_SHUTDOWN | EWX_FORCE,

            /// <summary>
            /// EWX_SHUTDOWN | EWX_FORCEIFHUNG
            /// </summary>
            EWX_FORCEIFHUNGSHUTDOWN = EWX_SHUTDOWN | EWX_FORCEIFHUNG,

            /// <summary>
            /// EWX_POWEROFF | EWX_FORCE
            /// </summary>
            EWX_FORCEPOWEROFF = EWX_POWEROFF | EWX_FORCE,

            /// <summary>
            /// EWX_POWEROFF | EWX_FORCEIFHUNG
            /// </summary>
            EWX_FORCEIFHUNGPOWEROFF = EWX_POWEROFF | EWX_FORCEIFHUNG,

            /// <summary>
            /// EWX_LOGOFF | EWX_FORCE
            /// </summary>
            EWX_FORCELOGOFF = EWX_LOGOFF | EWX_FORCE,

            /// <summary>
            /// EWX_LOGOFF | EWX_FORCEIFHUNG
            /// </summary>
            EWX_FORCEIFHUNGLOGOFF = EWX_LOGOFF | EWX_FORCEIFHUNG
        }

        /// <summary>
        /// Shuts down the machine according the given value.
        /// </summary>
        /// <param name="ewx_value">Set of flags defined in EWX_ENUM</param>
        public static void ExitSystem(EWX_ENUM ewx_value)
        {
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            LUID luid = new LUID();

            LookupPrivilegeValue(null, "SeShutdownPrivilege", ref luid);
            int processHandle = GetCurrentProcess();
            int tokenHandle = 0;
            OpenProcessToken(processHandle, 0x00000020 | 0x00000008, ref tokenHandle);

            tp.PriviledgeCount = 1;
            tp.Attributes = 0x00000002;
            tp.Luid = luid;

            int tpsz = Marshal.SizeOf(tp);
            AdjustTokenPrivileges(tokenHandle, 0, ref tp, tpsz, 0, 0);
            ExitwindowsEx((int)ewx_value, 0);
        }

        [DllImport("KERNEL32.DLL")]
        private static extern int GetCurrentProcess();

        [DllImport("advapi32.dll")]
        private static extern bool OpenProcessToken(int processHandle, uint desiredAccess, ref int tokenHandle);

        [DllImport("advapi32.dll")]
        private static extern bool LookupPrivilegeValue(string systemName, string name, ref LUID pluid);

        [DllImport("advapi32.dll")]
        private static extern int AdjustTokenPrivileges(
            int tokenHandler,
            int disableAllPrivileges,
            [MarshalAs(UnmanagedType.Struct)] ref TOKEN_PRIVILEGES newState,
            int bufferLength,
            int previousState,
            int returnLength);

        [DllImport("user32.dll")]
        private static extern bool ExitwindowsEx(int flg, int rea);

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID
        {
            public uint LowPart;
            public uint HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TOKEN_PRIVILEGES
        {
            public uint PriviledgeCount;
            public LUID Luid;
            public uint Attributes;
        }
    }
}
