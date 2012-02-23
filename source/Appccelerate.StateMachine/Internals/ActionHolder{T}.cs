// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionHolder{T}.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Internals
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Wraps an action with a single parameter.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the action.</typeparam>
    public class ActionHolder<T> : IActionHolder
    {
        /// <summary>
        /// the wrapped action
        /// </summary>
        private readonly Action<T> action;

        /// <summary>
        /// the parameter that is passed to the action on execution.
        /// </summary>
        private readonly T parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionHolder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="action">The wrapped action.</param>
        /// <param name="parameter">The parameter that is passed to the action on execution.</param>
        public ActionHolder(Action<T> action, T parameter)
        {
            this.action = action;
            this.parameter = parameter;
        }

        /// <summary>
        /// Executes the wrapped action.
        /// </summary>
        public void Execute()
        {
            this.action(this.parameter);
        }

        /// <summary>
        /// Describes the action.
        /// </summary>
        /// <returns>Description of the action.</returns>
        public string Describe()
        {
            return this.action.Method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() ? "anonymous" : this.action.Method.Name;
        }
    }
}