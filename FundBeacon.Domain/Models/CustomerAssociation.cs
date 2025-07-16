using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Domain.Models
{
    public class CustomerAssociation
    {
        [Key]
        public int CustomerAssociationId { get; set; }
        public int CustomerId { get; set; }
        public string AssociatedCode { get; set; }
        public DateTime AssociationStartDate { get; set; }
        public DateTime? AssociationEndDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
