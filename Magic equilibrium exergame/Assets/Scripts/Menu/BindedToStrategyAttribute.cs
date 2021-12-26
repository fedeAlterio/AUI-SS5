﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Menu
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BindedToStrategyAttribute : Attribute
    {
        public BindedToStrategyAttribute(Type t) => Type = t;
        public Type Type { get; }
    }
}
