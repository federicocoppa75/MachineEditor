﻿using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Inverters
{
    public class UpdateInverterMessage : BaseBackNotificationIdMessage
    {
        public int RotationSpeed { get; set; }
        public double Duration { get; set; }
    }
}
