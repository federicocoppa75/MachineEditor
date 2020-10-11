﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineEditor.Messages
{
    public class ChangeModel
    {
        public ModelVisual3D Model { get; set; }
        public ModelVisual3D OldModel { get; set; }
    }
}
