using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FibaroHomeCenter.Fibaro
{
    public class FibaroAPI
    {
        Devices mydevice;
        List<Devices> mydeviceList;


        T GetObject<T>(Dictionary<string, object> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type);

            mydevice = new Devices();
            foreach (var kv in dict)
            {
                var kk = mydevice.GetType().GetProperty(kv.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (kk != null)
                {
                    type.GetProperty(kv.Key).SetValue(obj, kv.Value);
                    if (kv.Key == "roomID")
                    {
                        Room newRoom = new Room() { id = Convert.ToInt16(kv.Value) };
                        newRoom = (Room)CallFibaroAPI(newRoom);
                        type.GetProperty("roomName").SetValue(obj, newRoom.name);

                    }
                }

            }
            return (T)obj;
        }

        public object CallFibaroAPI()
        {
            string value = ConfigurationManager.AppSettings["URL"];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["Url"] + @":80/api/devices");
            request.Credentials = new NetworkCredential { UserName = ConfigurationManager.AppSettings["Username"], Password = ConfigurationManager.AppSettings["Password"] };

            try
            {
                var stream = getHttpRequest(request);

                JArray v = JArray.Parse(stream.ToString());

                mydeviceList = new List<Devices>();
                foreach (JObject o in v.Children<JObject>())
                {
                    Dictionary<string, object> dictObj = o.ToObject<Dictionary<string, object>>();
                    mydeviceList.Add(GetObject<Devices>(dictObj));
                }
                return mydeviceList;
                //return JsonConvert.DeserializeObject(stream.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public object CallFibaroAPI(Room room)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["Url"] + @":80/api/rooms/" + room.id);
            request.Credentials = new NetworkCredential { UserName = ConfigurationManager.AppSettings["Username"], Password = ConfigurationManager.AppSettings["Password"] };

            try
            {
                var stream = getHttpRequest(request);
                return JsonConvert.DeserializeObject<Room>(stream.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public object CallFibaroAPI(bool motionsensor, long deviceID)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["Url"] + @":80/api/devices/" + deviceID);
            request.Credentials = new NetworkCredential { UserName = ConfigurationManager.AppSettings["Username"], Password = ConfigurationManager.AppSettings["Password"] };

            try
            {
                var stream = getHttpRequest(request);
                if (motionsensor == true)
                    return JsonConvert.DeserializeObject<MotionSensor>(stream.ToString());



                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static object getHttpRequest(HttpWebRequest request)
        {
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
    }
}