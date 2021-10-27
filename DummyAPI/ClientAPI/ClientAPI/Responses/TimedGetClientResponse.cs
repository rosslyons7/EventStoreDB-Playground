using ClientAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Responses {
    public class TimedGetClientResponse {

        public Client Client { get; set; }
        public string Time { get; set; }
        public int EventTotal { get; set; }
    }
}
