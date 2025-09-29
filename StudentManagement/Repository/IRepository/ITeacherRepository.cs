using StudentManagement.Model;

namespace StudentManagement.Repository.IRepository
{
    public interface ITeacherRepository
    {
        void Add(Teacher teacher);
        Teacher? GetByEmail(string email);
        Teacher? GetById(int Id);
        void Save();
    }
}
