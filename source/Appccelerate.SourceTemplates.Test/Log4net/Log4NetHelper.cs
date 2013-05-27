//-------------------------------------------------------------------------------
// <copyright file="Log4NetHelper.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Filter;

    public class Log4netHelper : IDisposable
    {
        private readonly MemoryAppender logAppender;

        public Log4netHelper()
            : this(new IFilter[] { })
        {
        }

        public Log4netHelper(string loggerName)
            : this(new IFilter[] { new LoggerMatchFilter { LoggerToMatch = loggerName } })
        {
        }

        public Log4netHelper(params IFilter[] logFilter)
        {
            this.logAppender = new MemoryAppender();

            foreach (IFilter filter in logFilter)
            {
                this.logAppender.AddFilter(filter);
            }

            if (logFilter.Any())
            {
                this.logAppender.AddFilter(new DenyAllFilter());
            }

            log4net.Config.BasicConfigurator.Configure(this.logAppender);
        }

        public void LogContains(string message)
        {
            this.LogContains(null, message);
        }

        public void LogContains(Level level, string message)
        {
            bool found = (from e in this.logAppender.GetEvents()
                          where (level == null || e.Level == level) && e.MessageObject.ToString().Contains(message)
                          select e).Any();

            if (!found)
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendFormat("Missing log message: level {0} searched message\n\r    {1}", level, message);
                errorMessage.AppendLine();
                this.DumpMessages(errorMessage);

                throw new Log4NetHelperException(errorMessage.ToString());
            }
        }

        public void LogMatch(string pattern)
        {
            this.LogMatch(null, pattern);
        }

        public void LogMatch(Level level, string pattern)
        {
            Regex regex = new Regex(pattern);

            bool found = (from e in this.logAppender.GetEvents()
                          where (level == null || e.Level == level) && regex.Match(e.MessageObject.ToString()).Success
                          select e).Any();

            if (!found)
            {
                StringBuilder message = new StringBuilder();
                message.AppendFormat("Missing log message: level {0} searched pattern\n\r    {1}", level, pattern);
                message.AppendLine();
                this.DumpMessages(message);

                throw new Log4NetHelperException(message.ToString());
            }
        }

        public void Dispose()
        {
            LogManager.ResetConfiguration();
        }

        public void DumpAllMessagesToConsole()
        {
            StringBuilder messages = new StringBuilder();
            this.DumpMessages(messages);

            Console.Write(messages);
        }

        private void DumpMessages(StringBuilder message)
        {
            message.AppendLine("Existing messages:");
            foreach (LoggingEvent loggingEvent in this.logAppender.GetEvents())
            {
                message.Append("    ");
                message.Append(loggingEvent.LoggerName);
                message.Append("    ");
                message.Append(loggingEvent.Level);
                message.Append("    ");
                message.AppendLine(loggingEvent.MessageObject.ToString());
            }
        }

        public class Log4NetHelperException : Exception
        {
            public Log4NetHelperException(string message) : base(message)
            {
            }
        }
    }
}