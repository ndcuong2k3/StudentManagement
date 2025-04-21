using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentManagementSystem.StudentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Student> tblHocSinh { get; set; }
    public DbSet<Class> tblLopHoc { get; set; }
    public DbSet<Teacher> tblGiaoVien { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasKey(s => s.iMaHS); 
        modelBuilder.Entity<Class>()
           .HasKey(s => s.iMaLop);
        modelBuilder.Entity<Teacher>().HasKey(s => s.iMaGV);
    }
}