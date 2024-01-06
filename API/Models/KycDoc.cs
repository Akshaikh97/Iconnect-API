using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class KycDoc
    {
        [Key]
        public int KycId { get; set; }
        public int RegId  { get; set; }
        public string Pan { get; set; }
        public string DocPathPan { get; set; }
        public string DocPanType { get; set; }
        public byte[] DocPan { get; set; }
        public string DocPathPhoto { get; set; }
        public string DocPhotoType { get; set; }
        public byte[] DocPhoto { get; set; }
        public string DocPathSign { get; set; }
        public string DocSignType { get; set; }
        public byte[] DocSign { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}