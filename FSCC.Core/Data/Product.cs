using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCC.Core.Data
{
    public partial class Product
    {
        public Product()
        {
            ProductReviews = new HashSet<ProductReview>();
        }

        [Key]
        public long ID { get; set; }
        public string ProductTitle { get; set; }
        [ForeignKey("ProductKind")]
        public long ProductKindID { get; set; }

        public virtual ProductKind ProductKind { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }
    }
}
