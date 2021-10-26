using Newtonsoft.Json;
using Producer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer {
    class Client {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime DateJoined { get; set; }
        public double InvestmentTotal { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void When(string jsonObj, string eventType) {
            switch (eventType) {
                case "create-client":
                    Apply(JsonConvert.DeserializeObject<ClientCreated>(jsonObj));
                    break;
                case "investment-deposit":
                    Apply(JsonConvert.DeserializeObject<ClientInvestmentDeposit>(jsonObj));
                    break;
                case "investment-withdrawal":
                    Apply(JsonConvert.DeserializeObject<ClientInvestmentWithdrawal>(jsonObj));
                    break;
                case "name-change":
                    Apply(JsonConvert.DeserializeObject<ClientNameChange>(jsonObj));
                    break;
            }
        }

        private void Apply(ClientCreated @event) {
            Console.WriteLine($"Initial Deposit: {@event.InvestmentTotal}");
            Id = @event.Id;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            Birthday = @event.Birthday;
            DateJoined = @event.DateJoined;
            InvestmentTotal = @event.InvestmentTotal;
            Email = @event.Email;
            Username = @event.Email;
            Password = @event.Password;
        }

        private void Apply(ClientInvestmentDeposit @event) {

            Console.WriteLine($"Deposit: {@event.AmountDeposited}");
            InvestmentTotal += @event.AmountDeposited;
        }

        private void Apply(ClientInvestmentWithdrawal @event) {

            Console.WriteLine($"Withdrawal: {@event.AmountWithdrawn}");
            InvestmentTotal -= @event.AmountWithdrawn;
        }

        private void Apply(ClientNameChange @event) {
            FirstName = @event.FirstName;
            LastName = @event.LastName;
        }
    }
}
