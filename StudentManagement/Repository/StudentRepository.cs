using StudentManagement.Data;
using StudentManagement.Model;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _db;

        public StudentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Student student)
        {
            _db.Update(student);
        }
    }
}
