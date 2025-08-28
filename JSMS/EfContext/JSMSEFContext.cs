using JSMS.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace JSMS.EfContext
{
    public class JSMSEFContext : DbContext
    {
        public JSMSEFContext(DbContextOptions<JSMSEFContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Founder> Founders { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Apply> Applies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table Names
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Founder>().ToTable("Founder");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Apply>().ToTable("Apply");

            modelBuilder.Entity<Founder>().HasData(
                new Founder { FdId=1,FdName = "Rich Danny", Fd_Company_Name = "IT Solution", Fd_Email = "ITSolution2000@gmail.com" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmpId = 1, EmpName = "Rahul", EmpResumeUrl = "/resumes/rahul.pdf", EmpEmail = "rahul@test.com", EmpExpriance = "2 Years", EmpEducation = "B.Tech" }
            );

            modelBuilder.Entity<Post>().HasData(
               new Post { Post_Id = 1, FdId = 1, Post_Name = "Software Developer", Post_Count = 2, Post_Description = "C# Developer with EF Core experience" }
            );

            modelBuilder.Entity<Apply>().HasData(
                new Apply { ApplyId = 1, EmpId = 1, Post_Id = 1, Status = true }
            );

            // Relationships
            modelBuilder.Entity<Post>()
                .HasOne<Founder>()
                .WithMany()
                .HasForeignKey(p => p.FdId);

            modelBuilder.Entity<Apply>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(a => a.EmpId);

            modelBuilder.Entity<Apply>()
                .HasOne<Post>()
                .WithMany()
                .HasForeignKey(a => a.Post_Id);
        }
    }
}
