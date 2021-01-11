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
    public partial class ProductReview
    {
        [Key]
        public long ID { get; set; }
        public string ReviewText { get; set; }
        public int ProductRating { get; set; }
        public DateTime ReviewDate { get; set; }
        [ForeignKey("Product")]
        public long ProductID { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
