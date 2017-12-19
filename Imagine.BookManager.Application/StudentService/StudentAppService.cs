using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Student;
using System;
using System.Linq;
using Abp.AutoMapper;

namespace Imagine.BookManager.StudentService
{
    public class StudentAppService : BookManagerAppServiceBase, IStudentAppService
    {
        private readonly IRepository<Student, Int64> _studentRepository;

        public StudentAppService(IRepository<Student, Int64> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public long CreateStudent(Student student)
        {
            var studentTemp = _studentRepository.FirstOrDefault(x => x.UserName == student.UserName);
            if (studentTemp != null)
            {
                throw new UserFriendlyException("The student already exists");
            }
            return _studentRepository.InsertAndGetId(student);
        }

        public PaginationDataList<StudentOut> GetAllStudentList(int? pageIndex, int? singletonPageCount = null)
        {
            var queryable = _studentRepository.GetAll().OrderByDescending(x => x.Id);
            return ObjectMapper.Map<PaginationDataList<StudentOut>>(
                 queryable.ToPagination(pageIndex, singletonPageCount)
                );
        }

        public void UpdateStudent(Student student)
        {
            var studentTemp = _studentRepository.FirstOrDefault(x => x.StudentId == student.StudentId);
            if (studentTemp == null)
            {
                throw new UserFriendlyException("The student does not exist");
            }
            studentTemp.FullName = student.FullName;
            studentTemp.UserName = student.UserName;
            studentTemp.Gender = student.Gender;
            studentTemp.IsDelete = student.IsDelete;
            studentTemp.Mobile = student.Mobile;
            studentTemp.GuardianName = student.GuardianName;
            studentTemp.Picture = student.Picture;
            studentTemp.DateOfBirth = student.DateOfBirth;
            studentTemp.Password = student.Password;
            _studentRepository.Update(studentTemp);
        }

        public bool CheckStudentName(string studentName)
        {
            var student = _studentRepository.FirstOrDefault(x => x.UserName == studentName);
            if (student == null)
                return true;
            return false;
        }
    }
}
