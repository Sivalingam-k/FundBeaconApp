using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Domain.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string CustomerCode { get; set; }
        public string UniqueId { get; set; }

        public ICollection<CustomerAssociation> Associations { get; set; }
    }
}
