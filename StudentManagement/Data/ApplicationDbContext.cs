using Microsoft.EntityFrameworkCore;
using StudentManagement.Model;

namespace StudentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
             
        }

        public DbSet<Student> Students{ get; set; }

        public DbSet<Teacher> Teachers { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 1,
                    Name = "Yogeshwar",
                    Email = "yogi@gmail.com",
                    Department = "Computers",
                    State = "Maharashta",
                    City = "Pune",
                    PostalCode = 411021
                }
                );
        }

    }
}
