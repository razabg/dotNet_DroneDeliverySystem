﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneToList
    {
        public int Id{ get; set; }
        public string Model{ get; set; }
        public string Weight{ get; set; }
        public double Battery{ get; set; }
        public string Status{ get; set; }
        public Location CurrentLocation{ get; set; }
        public int? IdOfParcelInTransfer {get;set; }

        public override string ToString()
        {
            string printInfo = " ";
            printInfo += $"ID:{Id}, ";
            printInfo += $"Model:{Model}, ";
            printInfo += $"Weight:{Weight}, ";
            printInfo += $"Battary:{Battery}%, ";
            printInfo += $"Status:{Status}, ";
            printInfo += $"Location is:{CurrentLocation} ";
            printInfo += $"the Id of the parcel in transport:{IdOfParcelInTransfer} ";
            return printInfo;
        }

    }
}
