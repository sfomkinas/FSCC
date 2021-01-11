using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCC.Core.Data
{
    public partial class JournalLog
    {
        [Key]
        public long ID { get; set; }
        public DateTime Date { get; set; }
        public string RequestType { get; set; }
        public string Cookies { get; set; }
        public string UrlReferrer { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string UserAgent { get; set; }
        public string Headers { get; set; }
    }
}
