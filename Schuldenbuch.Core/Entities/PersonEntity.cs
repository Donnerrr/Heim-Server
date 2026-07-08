using System;
using System.Collections.Generic;
using System.Text;

namespace Schuldenbuch.Core.Entities
{
    public class PersonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

        public int? UserId { get; set; }
        public UserEntity? User { get; set; }


        public ICollection<DebtEntity> Debts { get; set; } = new List<DebtEntity>();
    }
}
