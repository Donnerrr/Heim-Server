using System;
using System.Collections.Generic;
using System.Text;

namespace Schuldenbuch.Core.Entities
{
    public class DebtEntity
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public DateTime? PaidDate { get; set; }
        
        
        public int PersonId { get; set; }
        public PersonEntity? Person { get; set; }

    }
}
