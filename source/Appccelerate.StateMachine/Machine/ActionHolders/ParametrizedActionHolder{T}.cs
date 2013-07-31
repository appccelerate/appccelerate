// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParametrizedActionHolder{T}.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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
// --------------------------------------------------------------------------------------------------------------------

namespace Appccelerate.StateMachine.Machine.ActionHolders
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class ParametrizedActionHolder<T> : IActionHolder
    {
        private readonly Action<T> action;

        private readonly T parameter;

        public ParametrizedActionHolder(Action<T> action, T parameter)
        {
            this.action = action;
            this.parameter = parameter;
        }

        public void Execute(object argument)
        {
            this.action(this.parameter);
        }

        public string Describe()
        {
            StringBuilder result = new StringBuilder();

            result.Append(this.action.Method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() ? "anonymous" : this.action.Method.Name);

            if (this.parameter == null)
            {
                result.Append("(null)");
            }
            else
            {
                result.AppendFormat("({0})", this.parameter);
            }

            return result.ToString();
        }
    }
}