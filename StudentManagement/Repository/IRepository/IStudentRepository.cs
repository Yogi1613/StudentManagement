using StudentManagement.Model;

namespace StudentManagement.Repository.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        void Update(Student student);
    }
}
