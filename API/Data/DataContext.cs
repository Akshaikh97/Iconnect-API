using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext: DbContext
    {
         public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<GenerateOtp> GenerateOtp { get; set; }
        public DbSet<KycDoc> KycDoc { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}