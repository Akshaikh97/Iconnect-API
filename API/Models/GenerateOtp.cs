using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class GenerateOtp
    {
        [Key]
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string MessageId { get; set; }
        public int Otp { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}