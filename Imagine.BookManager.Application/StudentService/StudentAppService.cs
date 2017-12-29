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
            name = Guard.EnsureParam(name);
            var admin = AdminRepository.FirstOrDefault(e => e.UserId == userId);
            if (admin == null)
                return new PaginationDataList<StudentOut>() { CurrentPage = 0, ListData = new List<StudentOut>(), TotalPages = 0 };
            var classList = ClassRepository.GetAllIncluding(x => x.Students).Where(e => e.InstitutionId == admin.InstitutionId);

            classList = FilterStudentByClassId(classId, classList);

            var studentList = classList.SelectMany(x => x.Students);
            studentList = studentList.Where(e => e.UserName.Contains(name) && e.Mobile.Contains(mobile));
            studentList = FilterStudentByStartTime(startTime, studentList);

            var studenAllocationList = studentList.SelectMany(e => e.StudentAllocations);

            IQueryable<StudentOut> studentOutList = FillData(studentList, studenAllocationList);

            studentOutList = FilterStudentBySetId(studentOutList, setId);
            studentOutList = FilterStudentBySetStatus(studentOutList, setStatus);

            return studentOutList.ToPagination(pageSize, pageRows);
        }

        private IQueryable<StudentOut> FillData(IQueryable<Student> studentList, IQueryable<StudentAllocation> studenAllocationList)
        {
            IQueryable<StudentOut> studentOutList = ObjectMapper.Map<List<StudentOut>>(studentList).AsQueryable();
            foreach (var studentOut in studentOutList)
            {
                var allocationList = studenAllocationList.Where(e => e.StudentId == studentOut.StudentId);
                if (allocationList.Count() == 0)
                {
                    studentOut.SetNames.Add(string.Empty);
                    studentOut.SetStatus.Add(HintInfo.UnAllocated);
                    continue;
                }
                foreach (var allocation in allocationList)
                {
                    var teacherAllocation = TeacherAllocationRepository.FirstOrDefault(e => e.Id == allocation.TeacherAllocationId);
                    if (teacherAllocation == null)
                        continue;
                    var setInfo = SetRepository.FirstOrDefault(e => e.Id == teacherAllocation.SetId);
                    if (setInfo == null)
                        continue;

                    studentOut.SetNames.Add(setInfo.SetName);
                    if (DateTime.Now.Date > allocation.ExpiryDate.Date)
                        studentOut.SetStatus.Add(HintInfo.SetExpire);
                    else
                        studentOut.SetStatus.Add(HintInfo.Allocated);
                    studentOut.SetIds.Add(setInfo.Id);
                }
            }
            return studentOutList;
        }

        private IQueryable<Student> FilterStudentByStartTime(DateTime? startTime, IQueryable<Student> studentList)
        {
            if (startTime.HasValue)
                studentList = studentList.Where(e => DbFunctions.DiffDays(e.DateCreated, startTime.Value) == 0).OrderByDescending(e => e.DateCreated);
            return studentList;
        }

        private IQueryable<ClassInfo> FilterStudentByClassId(int? classId, IQueryable<ClassInfo> classList)
        {
            if (classId.HasValue && classId.Value != 0)
                classList = classList.Where(e => e.Id == classId.Value);
            return classList;
        }

        private IQueryable<StudentOut> FilterStudentBySetId(IQueryable<StudentOut> studentOutList, int? setId)
        {
            if (setId.HasValue && setId.Value != 0)
                studentOutList = studentOutList.Where(e => e.SetIds.Contains(setId.Value));
            return studentOutList;
        }

        private IQueryable<StudentOut> FilterStudentBySetStatus(IQueryable<StudentOut> studentOutList, int setStatus)
        {
            switch (setStatus)
            {
                case 1:
                    return studentOutList.Where(e => e.SetStatus.Contains(HintInfo.Allocated));
                case 2:
                    return studentOutList.Where(e => e.SetStatus.Contains(HintInfo.UnAllocated));
                case 3:
                    return studentOutList.Where(e => e.SetStatus.Contains(HintInfo.SetExpire));
                default:
                    return studentOutList;
            }
        }
    }
}
