using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCC.Core.Data
{
    public partial class ProductKind
    {
        public ProductKind()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public long ID { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
