using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Student;
using System;
using System.Linq;
using Abp.AutoMapper;
using System.Collections.Generic;

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

        public IRepository<StudentAllocation, Int64> StudentAllocationRepository { get; set; }

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
                return new PaginationDataList<StudentOut>() { CurrentPage = pageSize ?? 0, ListData = new List<StudentOut>(), TotalPages = 0 };
            var classList = ClassRepository.GetAllList(e => e.InstitutionId == admin.InstitutionId);
            if (classId.HasValue && classId.Value != 0)
                classList = classList.Where(e => e.Id == classId.Value).ToList();
            List<Student> studentList = new List<Student>();
            foreach (var item in classList)
            {
                var list = _studentRepository.GetAllList(e => e.ClassId == item.Id);
                if (list.Count == 0)
                    continue;
                studentList.AddRange(list);
            }
            studentList = studentList.Where(e => e.UserName.Contains(name) && e.Mobile.Contains(mobile)).ToList();
            if (startTime.HasValue)
                studentList = studentList.Where(e => e.DateCreated.Date == startTime.Value.Date).OrderByDescending(e => e.DateCreated).ToList();
            List<StudentOut> studentOutList = ObjectMapper.Map<List<StudentOut>>(studentList);
            foreach (var item in studentOutList)
            {
                item.SetStatus = new List<string>();
                item.SetNames = new List<string>();
                item.SetIds = new List<int>();
                var list = StudentAllocationRepository.GetAllList(e => e.StudentId == item.StudentId);
                if (list.Count == 0)
                {
                    item.SetNames.Add(string.Empty);
                    item.SetStatus.Add("未分配");
                    continue;
                }
                foreach (var item2 in list)
                {
                    var teacherAllocation = TeacherAllocationRepository.FirstOrDefault(e => e.Id == item2.TeacherAllocationId);
                    if (teacherAllocation == null)
                        continue;
                    var setInfo = SetRepository.FirstOrDefault(e => e.Id == setId);
                    if (setInfo == null)
                        continue;

                    item.SetNames.Add(setInfo.SetName);
                    if (item2.ExpiryDate.Date > DateTime.Now.Date)
                        item.SetStatus.Add("绘本过期");
                    else
                        item.SetStatus.Add("已分配");
                    item.SetIds.Add(setInfo.Id);
                }
            }
            if (setId.HasValue && setId.Value != 0)
                studentOutList = studentOutList.Where(e => e.SetIds.Contains(setId.Value)).ToList();

            if (setStatus == 1)
                studentOutList = studentOutList.Where(e => e.SetStatus.Contains("已分配")).ToList();
            else if (setStatus == 2)
                studentOutList = studentOutList.Where(e => e.SetStatus.Contains("未分配")).ToList();
            else if (setStatus == 3)
                studentOutList = studentOutList.Where(e => e.SetStatus.Contains("绘本过期")).ToList();

            return studentOutList.AsQueryable().ToPagination(pageSize, pageRows);
        }
    }
}
