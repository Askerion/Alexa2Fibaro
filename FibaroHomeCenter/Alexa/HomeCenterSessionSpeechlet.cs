using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.UI;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FibaroHomeCenter.Fibaro;
using System.Collections;
using System.Configuration;

namespace FibaroHomeCenter.Alexa
{
    public class HomeCenterSessionSpeechlet : Speechlet
    {

        private static Logger Log = LogManager.GetCurrentClassLogger();
        FibaroAPI fibaroapi = new FibaroAPI();

        private string NAME_KEY = "name";
        private string NAME_SLOT = "Name";



        public override void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session)
        {
            Log.Info("OnSessionStarted requestId={0}, sessionId={1}", sessionStartedRequest.RequestId, session.SessionId);
        }

        public override SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session)
        {
            Log.Info("OnLaunch requestId={0}, sessionId={1}", launchRequest.RequestId, session.SessionId);
            return GetWelcomeResponse();
        }

        /**
         * Creates and returns a {@code SpeechletResponse} with a welcome message.
         * 
         * @return SpeechletResponse spoken and visual welcome message
         */
        private SpeechletResponse GetWelcomeResponse()
        {
            // Create the welcome message.
            string speechOutput =
                "Willkommen im HomeCenter von Rene Steinbach, mit diesem APP, kannste du Daten von deinen Fibaro HomeCenter abfragen";

            // Here we are setting shouldEndSession to false to not end the session and
            // prompt the user for input
            return BuildSpeechletResponse("Welcome", speechOutput, false);
        }

        public override SpeechletResponse OnIntent(IntentRequest intentRequest, Session session)
        {
            Log.Info("OnIntent requestId={0}, sessionId={1}", intentRequest.RequestId, session.SessionId);

            // Get intent from the request object.
            Intent intent = intentRequest.Intent;
            string intentName = (intent != null) ? intent.Name : null;

            // Note: If the session is started with an intent, no welcome message will be rendered;
            // rather, the intent specific response will be returned.
            if ("HomeCenter".Equals(intentName))
            {

                Dictionary<string, Slot> slots = intent.Slots;

                foreach(var item in intent.Slots)
                {
                    if(item.Key == "Temperatur")
                    {
                        NAME_SLOT = "Temperatur";
                        return FindDeviceandResponse(intent, session, item.Value);
                    }
                }

                return null;

            }
            else if ("WhatsMyNameIntent".Equals(intentName))
            {
                return null;
            }
            else {
                throw new SpeechletException("Invalid Intent");
            }
        }

        private SpeechletResponse FindDeviceandResponse(Intent intent, Session session, Slot slot)
        {
            // Get the slots from the intent.
            //Dictionary<string, Slot> slots = intent.Slots;

            // Get the name slot from the list slots.
            //Slot nameSlot = slots[NAME_SLOT];
            string speechOutput = "";

            // Check for name and create output to user.
            if (NAME_SLOT != null && NAME_SLOT == "Temperatur")
            {

                //string name = slot.Value; // Zimmername

                //Get all Devices from API                
                List<Devices> allDevices = ((List<Devices>)fibaroapi.CallFibaroAPI()).Cast<Devices>().ToList();
                //Delete Devices from List
                allDevices.RemoveAll(x => !x.roomName.Contains(slot.Value, StringComparison.OrdinalIgnoreCase));
                //Get alle Temperatur Devices from Web Config
                var paths = new List<string>(ConfigurationManager.AppSettings["Temperatur"].Split(new char[] { ';' }));

                //Find Device Where Typ in Web Config
                Devices singleDevice = inList(paths, allDevices);

                //Generate Response 
                if (singleDevice != null)
                {
                    //Get MotionSensor
                    MotionSensor getMotionSensor = (MotionSensor)fibaroapi.CallFibaroAPI(true, singleDevice.id);


                    // Store the user's name in the Session and create response.
                    //string name = nameSlot.Value; // Zimmername
                    session.Attributes[NAME_KEY] = slot.Value;
                    speechOutput = String.Format(
                        "Im {0} sind es {1} Grad", slot.Value, getMotionSensor.properties.value);
                }
                else
                {
                    // Kein Temperaturgerät in diesem Zimmer gefunden.
                    speechOutput = "In diesem Zimmer habe ich kein Gerät gefunden!";
                }


            }
            else {
                // Render an error since we don't know what the users name is.
                speechOutput = "I'm not sure what your name is, please try again";
            }

            // Here we are setting shouldEndSession to false to not end the session and
            // prompt the user for input
            return BuildSpeechletResponse(intent.Name, speechOutput, false);
        }

        Devices inList(List<string> searchString, List<Devices> saerchDevices)
        {
            foreach (string item in searchString)
            {
                Devices bc = saerchDevices.Find(c => c.type == item);

                return bc;
            }

            return null;
        }



        public override void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session)
        {
            throw new NotImplementedException();
        }



        /**
         * Creates and returns the visual and spoken response with shouldEndSession flag
         * 
         * @param title
         *            title for the companion application home card
         * @param output
         *            output content for speech and companion application home card
         * @param shouldEndSession
         *            should the session be closed
         * @return SpeechletResponse spoken and visual response for the given input
         */
        private SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession)
        {
            // Create the Simple card content.
            SimpleCard card = new SimpleCard();
            card.Title = String.Format("SessionSpeechlet - {0}", title);
            //card.Subtitle = String.Format("SessionSpeechlet - Sub Title");
            card.Content = String.Format("SessionSpeechlet - {0}", output);

            // Create the plain text output.
            PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
            speech.Text = output;

            // Create the speechlet response.
            SpeechletResponse response = new SpeechletResponse();
            response.ShouldEndSession = shouldEndSession;
            response.OutputSpeech = speech;
            response.Card = card;
            return response;
        }

    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}