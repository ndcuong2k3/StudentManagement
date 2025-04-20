using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentManagementSystem.StudentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Student> tblHocSinh { get; set; }
    public DbSet<Class> tblLopHoc { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasKey(s => s.iMaHS); // Explicitly defining the primary key
        modelBuilder.Entity<Class>()
           .HasKey(s => s.iMaLop);
    }
}