﻿// Copyright 2012-2017 Dmitry Kischenko
//
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either 
// express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
using System;
using xFunc.Maths.Expressions;

namespace xFunc.ViewModels
{
    
    public class FunctionViewModel
    {

        private UserFunction function;
        private IExpression value;

        public FunctionViewModel(UserFunction function, IExpression value)
        {
            this.function = function;
            this.value = value;
        }

        public UserFunction Function
        {
            get
            {
                return function;
            }
        }

        public string Value
        {
            get
            {
                return value.ToString();
            }
        }

    }

}