namespace Parking.App.Common.ViewModels
{
    public class AuthenticateEventResponse
    {
        public int CommanType { get; set; }
        public int respCommand { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string EVENT_TIME { get; set; }
        public string USER_ID { get; set; }
        public string TYPE { get; set; }
        public string TEMPERATURE { get; set; }
        public string FACE_DATA { get; set; }
        public string MESSAGE { get; set; }
        public bool SUCCESS { get; set; }
    }
}
