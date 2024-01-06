using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Migrate;

public partial class IconnectDbContext : DbContext
{
    public IconnectDbContext()
    {
    }

    public IconnectDbContext(DbContextOptions<IconnectDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BSS-IT-DT-365\\MSSQLSERVER01;Database=IconnectDb;Encrypt=true;TrustServerCertificate=true;Integrated Security=True;MultipleActiveResultSets=True;App=EntityFramework");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
