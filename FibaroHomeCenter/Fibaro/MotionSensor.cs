using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FibaroHomeCenter.Fibaro
{

    public class MotionSensor
    {
        public int id { get; set; }
        public string name { get; set; }
        public int roomID { get; set; }
        public string type { get; set; }
        public string baseType { get; set; }
        public bool enabled { get; set; }
        public bool visible { get; set; }
        public bool isPlugin { get; set; }
        public int parentId { get; set; }
        public int remoteGatewayId { get; set; }
        public string[] interfaces { get; set; }
        public Properties properties { get; set; }
        public Actions actions { get; set; }
        public int created { get; set; }
        public int modified { get; set; }
        public int sortOrder { get; set; }


        public class Properties
        {
            public Parameter[] parameters { get; set; }
            public string zwaveCompany { get; set; }
            public string zwaveInfo { get; set; }
            public string zwaveVersion { get; set; }
            public int wakeUpTime { get; set; }
            public int pollingTimeSec { get; set; }
            public string batteryLevel { get; set; }
            public string batteryLowNotification { get; set; }
            public string configured { get; set; }
            public string dead { get; set; }
            public string defInterval { get; set; }
            public string deviceControlType { get; set; }
            public string deviceIcon { get; set; }
            public string emailNotificationID { get; set; }
            public string emailNotificationType { get; set; }
            public string endPointId { get; set; }
            public string liliOffCommand { get; set; }
            public string liliOnCommand { get; set; }
            public string log { get; set; }
            public string logTemp { get; set; }
            public string manufacturer { get; set; }
            public string markAsDead { get; set; }
            public string maxInterval { get; set; }
            public string minInterval { get; set; }
            public string model { get; set; }
            public string nodeId { get; set; }
            public string offset { get; set; }
            public string parametersTemplate { get; set; }
            public string productInfo { get; set; }
            public string pushNotificationID { get; set; }
            public string pushNotificationType { get; set; }
            public string remoteGatewayId { get; set; }
            public string saveLogs { get; set; }
            public string serialNumber { get; set; }
            public string showFireAlarm { get; set; }
            public string showFreezeAlarm { get; set; }
            public string smsNotificationID { get; set; }
            public string smsNotificationType { get; set; }
            public string stepInterval { get; set; }
            public string unit { get; set; }
            public string useTemplate { get; set; }
            public string userDescription { get; set; }
            public string value { get; set; }
        }

        public class Parameter
        {
            public int id { get; set; }
            public int size { get; set; }
            public int value { get; set; }
        }

        public class Actions
        {
            public int reconfigure { get; set; }
            public int setInterval { get; set; }
        }

    }
}