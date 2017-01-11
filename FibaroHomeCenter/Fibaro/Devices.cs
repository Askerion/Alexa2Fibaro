using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FibaroHomeCenter.Fibaro
{
    public class Devices
    {
        public long id { get; set; }
        public string name { get; set; }
        public long roomID { get; set; }
        public string roomName { get; set; }
        public string type { get; set; }
        public string baseType { get; set; }
        public bool enabled { get; set; }
        public bool visible { get; set; }
        public bool isPlugin { get; set; }
        public long parentId { get; set; }

    }
}