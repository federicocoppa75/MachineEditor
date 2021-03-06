﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoIstructionAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class EDKConverterAttribute : SetVariableIstructionConverterAttribute
    {
        public EDKConverterAttribute(int index) : base("EDK", index)
        {
        }
    }
}
