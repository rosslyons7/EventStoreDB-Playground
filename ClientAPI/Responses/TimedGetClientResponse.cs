using ClientAPI.Entities;

namespace ClientAPI.Responses {
    public class TimedGetClientResponse {

        public Client Client { get; set; }
        public string Time { get; set; }
        public int EventTotal { get; set; }
    }
}
