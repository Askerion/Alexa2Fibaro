using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FibaroHomeCenter.Fibaro
{
    public class Room
    {
        public int id { get; set; }
        public string name { get; set; }
        public int sectionID { get; set; }
        public string icon { get; set; }
        public Defaultsensors defaultSensors { get; set; }
        public int defaultThermostat { get; set; }
        public int sortOrder { get; set; }


        public class Defaultsensors
        {
            public int temperature { get; set; }
            public int humidity { get; set; }
            public int light { get; set; }
        }

    }
}