//-------------------------------------------------------------------------------
// <copyright file="GraphicHelper.cs" company="Appccelerate">
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
    /// Provides functionality for access graphical information.
    /// </summary>
    public static class GraphicHelper
    {
        /// <summary>
        /// Calculates the DPI of the windows desktop.
        /// </summary>
        /// <returns>The number of DPIs of the windows desktop</returns>
        public static int GetDpi()
        {
            IntPtr dPC = NativeMethods.GetDC(NativeMethods.GetDesktopWindow());
            int dpi = NativeMethods.GetDeviceCaps(dPC, 88);
            NativeMethods.ReleaseDC(NativeMethods.GetDesktopWindow(), dPC);
            return dpi;
        }

        internal static class NativeMethods
        {
            [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
            internal static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll")]
            internal static extern IntPtr GetDC(IntPtr ptr);

            [DllImport("gdi32.dll")]
            internal static extern int GetDeviceCaps(IntPtr handleDesktop, int index);

            [DllImport("user32.dll")]
            internal static extern int ReleaseDC(IntPtr handleWindow, IntPtr handleDesktop);
        }
    }
}
