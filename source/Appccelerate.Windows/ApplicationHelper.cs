//-------------------------------------------------------------------------------
// <copyright file="ApplicationHelper.cs" company="Appccelerate">
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
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides functionality in the scope of applications.
    /// </summary>
    public static class ApplicationHelper
    {
        /// <summary>
        /// Checks whether another instance of the same application is already running.
        /// </summary>
        /// <param name="switchToAlreadyRunningProcess">Whether the already running process is flashed and brought to front.</param>
        /// <returns>Whether another instance of the application is already running.</returns>
        public static bool CheckApplicationAlreadyRunning(bool switchToAlreadyRunningProcess)
        {
            Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);

            // > 1 because if there is already a process running, then 2 instances (with this one) are present
            if (processes.Length > 1)
            {
                if (switchToAlreadyRunningProcess)
                {
                    IntPtr hwnd = processes[0].Id != Process.GetCurrentProcess().Id ? processes[0].MainWindowHandle : processes[1].MainWindowHandle;

                    if (!NativeMethods.IsWindowVisible(hwnd))
                    {
                        NativeMethods.ShowWindowAsync(hwnd, 1); // maximize window
                    }

                    NativeMethods.SwitchToThisWindow(hwnd, true);
                }

                return true;
            }

            return false;
        }

        internal static class NativeMethods
        {
            /// <summary>
            /// EXTERN
            /// The SwitchToThisWindow function is called to switch focus to a specified window and bring it to the foreground.
            /// </summary>
            /// <param name="windowHandle">Handle to the window being switched to.</param>
            /// <param name="altTab">A TRUE for this parameter indicates that the window is being switched to using the Alt/Ctrl+Tab key sequence. This parameter should be FALSE otherwise.</param>
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern void SwitchToThisWindow(IntPtr windowHandle, bool altTab);

            /// <summary>
            /// The ShowWindowAsync function sets the show state of a window created by a different thread.
            /// </summary>
            /// <param name="windowHandle">Handle to the window.</param>
            /// <param name="showWindowCommand">Specifies how the window is to be shown. For a list of possible values, see the description of the ShowWindow function.</param>
            /// <returns>The asynchronous handle</returns>
            [DllImport("User32.dll")]
            internal static extern int ShowWindowAsync(IntPtr windowHandle, int showWindowCommand);

            /// <summary>
            /// The IsWindowVisible function retrieves the visibility state of the specified window.
            /// </summary>
            /// <param name="windowHandle">Handle to the window to test.</param>
            /// <returns>
            /// If the specified window, its parent window, its parent's parent window, and so forth, have the WS_VISIBLE style, the return value is nonzero. 
            /// Otherwise, the return value is zero. Because the return value specifies whether the window has the WS_VISIBLE style, it may be nonzero even if 
            /// the window is totally obscured by other windows.
            /// </returns>
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern bool IsWindowVisible(IntPtr windowHandle);
        }
    }
}