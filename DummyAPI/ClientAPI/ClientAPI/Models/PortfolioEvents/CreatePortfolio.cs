using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Models.PortfolioEvents {
    public record CreatePortfolio {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ClientId { get; set; }
        public string ClientDisplayName { get; set; }
    }
}
