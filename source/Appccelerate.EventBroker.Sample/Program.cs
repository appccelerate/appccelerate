//-------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Sample
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Main entry point
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EventBroker eventBroker = new EventBroker();

            // add log extension to the event broker to get log messages
            eventBroker.AddExtension(new DebugLogger());

            Pong oponent = new Pong();
            Ping form = new Ping();

            eventBroker.Register(oponent);
            eventBroker.Register(form);

            Application.Run(form);
        }
    }
}