using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Pan { get; set; }
        public string Password { get; set; }
        public bool LoginStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int RejectedBy { get; set; }
        public DateTime RejectedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}