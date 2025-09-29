using StudentManagement.Data;
using StudentManagement.Model;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Service
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentRepository.GetAll();

        }
        public Student? GetStudentById(int id)
        {
           Student? studentFromDb = _studentRepository.Get(u => u.Id == id);
            if (studentFromDb == null)
            {
                return null;
            }
            return studentFromDb;
        }
        public Student GetStudentByName(string name)
        {
            return _studentRepository.Get(u => u.Name == name);
        }
        public void AddStudent(Student student)
        {
            _studentRepository.Add(student);
            _studentRepository.Save();
        }
        public void UpdateStudent(Student student)
        {
            Student studentFromDb = _studentRepository.Get(u => u.Id == student.Id);

            if (studentFromDb!=null)
            {
                studentFromDb.Name = student.Name;
                studentFromDb.Email = student.Email;
                studentFromDb.Department= student.Department;
                studentFromDb.State = student.State;
                studentFromDb.PostalCode  = student.PostalCode;
                studentFromDb.City = student.City;

                _studentRepository.Update(studentFromDb);
            }
            _studentRepository.Save();

        }

        public void DeleteStudentById(int id)
        {
            var studentToBeDeleted = _studentRepository.Get(u=>u.Id==id);
            _studentRepository.Remove(studentToBeDeleted);
            _studentRepository.Save();

        }
        public void DeleteStudentByName(string name)
        {
            var studentToBeDeleted = _studentRepository.Get(u => u.Name == name);
            _studentRepository.Remove(studentToBeDeleted);
            _studentRepository.Save();

        }
    }
}
