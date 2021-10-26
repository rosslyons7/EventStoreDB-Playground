using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Models {
    public record ClientNameChange {

        public string FirstName {get; set;}
        public string LastName { get; set; }
    }
}
