﻿//-------------------------------------------------------------------------------
// <copyright file="Missable.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Machine
{
    using System;

    public class Missable<T>
    {
        private T value;

        public Missable()
        {
            this.IsMissing = true;
        }

        public Missable(T value)
        {
            this.Value = value;
        }

        public bool IsMissing { get; private set; }

        public T Value
        {
            get
            {
                if (this.IsMissing)
                {
                    throw new InvalidOperationException("a missing value cannot be accessed.");
                }

                return this.value;
            }

            set
            {
                this.value = value;

                this.IsMissing = false;
            }
        }
    }
}