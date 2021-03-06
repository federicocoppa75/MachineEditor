﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages.Inserter
{
    public class InsertMessage
    {
        public Point3D Position { get; set; }
        public Vector3D Direction { get; set; }
        public Color Color { get; set; }
        public double Length { get; set; }
        public double Diameter { get; set; }
    }
}
