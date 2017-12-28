using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Student;
using System;
using System.Linq;
using Abp.AutoMapper;
using System.Collections.Generic;
using System.Data.Entity;

namespace Imagine.BookManager.StudentService
{
    public class StudentAppService : BookManagerAppServiceBase, IStudentAppService
    {
        private readonly IRepository<Student, Int64> _studentRepository;

        public StudentAppService(IRepository<Student, Int64> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IRepository<Admin> AdminRepository { get; set; }

        public IRepository<ClassInfo> ClassRepository { get; set; }

        public IRepository<TeacherAllocation, Int64> TeacherAllocationRepository { get; set; }

        public IRepository<Set> SetRepository { get; set; }

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

        public PaginationDataList<StudentOut> SearchStudent(int? pageSize, int? pageRows, string name, int? classId,
            int setStatus, int? setId, string mobile, DateTime? startTime, Guid userId)
        {
            var admin = AdminRepository.FirstOrDefault(e => e.UserId == userId);
            if (admin == null)
                return new PaginationDataList<StudentOut>() { CurrentPage = 0, ListData = new List<StudentOut>(), TotalPages = 0 };
            var classList = ClassRepository.GetAllIncluding(x => x.Students).Where(e => e.InstitutionId == admin.InstitutionId);

            classList = SearchStudentByClassId(classId, classList);

            var studentList = classList.SelectMany(x => x.Students);
            studentList = studentList.Where(e => e.UserName.Contains(name) && e.Mobile.Contains(mobile));
            studentList = SearchStudentByStartTime(startTime, studentList);

            var studenAllocationList = studentList.SelectMany(e => e.StudentAllocations);

            IQueryable<StudentOut> studentOutList = FillData(studentList, studenAllocationList);

            studentOutList = SearchStudentBySetId(studentOutList, setId);
            studentOutList = SearchStudentBySetStatus(studentOutList, setStatus);

            return studentOutList.ToPagination(pageSize, pageRows);
        }

        private IQueryable<StudentOut> FillData(IQueryable<Student> studentList, IQueryable<StudentAllocation> studenAllocationList)
        {
            IQueryable<StudentOut> studentOutList = ObjectMapper.Map<List<StudentOut>>(studentList).AsQueryable();
            foreach (var item in studentOutList)
            {
                var list = studenAllocationList.Where(e => e.StudentId == item.StudentId);
                if (list.Count() == 0)
                {
                    item.SetNames.Add(string.Empty);
                    item.SetStatus.Add(HintInfo.UnAllocated);
                    continue;
                }
                foreach (var item2 in list)
                {
                    var teacherAllocation = TeacherAllocationRepository.FirstOrDefault(e => e.Id == item2.TeacherAllocationId);
                    if (teacherAllocation == null)
                        continue;
                    var setInfo = SetRepository.FirstOrDefault(e => e.Id == teacherAllocation.SetId);
                    if (setInfo == null)
                        continue;

                    item.SetNames.Add(setInfo.SetName);
                    if (DateTime.Now.Date > item2.ExpiryDate.Date)
                        item.SetStatus.Add(HintInfo.SetExpire);
                    else
                        item.SetStatus.Add(HintInfo.Allocated);
                    item.SetIds.Add(setInfo.Id);
                }
            }
            return studentOutList;
        }

        private static IQueryable<Student> SearchStudentByStartTime(DateTime? startTime, IQueryable<Student> studentList)
        {
            if (startTime.HasValue)
                studentList = studentList.Where(e => DbFunctions.DiffDays(e.DateCreated, startTime.Value) == 0).OrderByDescending(e => e.DateCreated);
            return studentList;
        }

        private static IQueryable<ClassInfo> SearchStudentByClassId(int? classId, IQueryable<ClassInfo> classList)
        {
            if (classId.HasValue && classId.Value != 0)
                classList = classList.Where(e => e.Id == classId.Value);
            return classList;
        }

        private IQueryable<StudentOut> SearchStudentBySetId(IQueryable<StudentOut> studentOutList, int? setId)
        {
            if (setId.HasValue && setId.Value != 0)
                studentOutList = studentOutList.Where(e => e.SetIds.Contains(setId.Value));
            return studentOutList;
        }

        private IQueryable<StudentOut> SearchStudentBySetStatus(IQueryable<StudentOut> studentOutList, int setStatus)
        {
            if (setStatus == 1)
                studentOutList = studentOutList.Where(e => e.SetStatus.Contains(HintInfo.Allocated));
            else if (setStatus == 2)
                studentOutList = studentOutList.Where(e => e.SetStatus.Contains(HintInfo.UnAllocated));
            else if (setStatus == 3)
                studentOutList = studentOutList.Where(e => e.SetStatus.Contains(HintInfo.SetExpire));
            return studentOutList;
        }
    }
}
