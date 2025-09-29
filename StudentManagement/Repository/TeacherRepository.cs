using StudentManagement.Data;
using StudentManagement.Model;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _db;
        public TeacherRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Add(Teacher teacher)
        {
            _db.Add(teacher);
        }

        public Teacher? GetByEmail(string email)
        {
          return _db.Teachers.Where(u => u.Email == email).FirstOrDefault();
        }

        public Teacher? GetById(int id)
        {
            return _db.Teachers.Where(u=>u.Id==id).FirstOrDefault();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
