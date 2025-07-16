using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Domain.Models
{
    public class ContactAddressAssociation
    {
        [Key]
        public int ContactAddressAssociationId { get; set; }
        public int AddressId { get; set; }
        public string AssociatedCode { get; set; }
        public DateTime AssocationStartDate { get; set; }
        public DateTime? AssocationEndDate { get; set; }

        public virtual ContactAddress Address { get; set; }
    }
}
