using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Admin;
using Imagine.BookManager.Dto.Class;
using System;
using System.Linq;

namespace Imagine.BookManager.ClassService
{
    public class ClassAppService : BookManagerAppServiceBase, IClassAppService
    {
        private readonly IRepository<ClassInfo> _classRepository;

        public ClassAppService(IRepository<ClassInfo> classRepository)
        {
            _classRepository = classRepository;
        }

        public IRepository<Admin> AdminRepository { get; set; }

        public IRepository<Student, Int64> StudentRepository { get; set; }

        public int CreateClassInfo(ClassInfoIn classInfo)
        {
            var admin = AdminRepository.FirstOrDefault(x => x.UserId == classInfo.UserId);
            if (admin == null)
            {
                return 0;
            }
            if (admin.InstitutionId.HasValue == false)
            {
                return 0;
            }
            var classInfoTemp = ObjectMapper.Map<ClassInfo>(classInfo);
            classInfoTemp.InstitutionId = admin.InstitutionId.Value;
            classInfoTemp.Admins.Add(admin);
            return _classRepository.InsertAndGetId(classInfoTemp);
        }

        public bool AllocationClassInfoToTeacher(int classId, Guid userId)
        {
            var admin = AdminRepository.GetAllIncluding(x => x.Classes).FirstOrDefault(x => x.UserId == userId);
            if (admin == null)
            {
                return false;
            }
            var classInfo = _classRepository.GetAllIncluding(x => x.Admins).FirstOrDefault(x => x.Id == classId);
            if (classInfo == null)
                return false;
            if (classInfo.InstitutionId != admin.InstitutionId)
                return false;
            var temp = classInfo.Admins.FirstOrDefault(x => x.UserId == userId);
            if (temp != null && temp.Id > 0)
            {
                return false;
            }
            classInfo.Admins.Add(admin);
            return _classRepository.Update(classInfo) != null;
        }

        public bool DeleteUsersClassInfo(int classId, Guid userId)
        {
            var admin = AdminRepository.GetAllIncluding(x => x.Classes).FirstOrDefault(x => x.UserId == userId);
            if (admin == null)
            {
                return false;
            }
            var classInfo = _classRepository.GetAllIncluding(x => x.Admins).FirstOrDefault(x => x.Id == classId);
            if (classInfo == null)
                return false;
            var temp = classInfo.Admins.FirstOrDefault(x => x.UserId == userId);
            if (temp == null || temp.Id == 0)
                return false;
            classInfo.Admins.Remove(admin);
            return _classRepository.Update(classInfo) != null;
        }

        public ClassInfoOut GetClassInfoById(int classId)
        {
            var classInfo = _classRepository.FirstOrDefault(classId);
            return ObjectMapper.Map<ClassInfoOut>(classInfo);
        }

        public PaginationDataList<AdminDto> GetClassInfoTeachers(
            int classId,
            int? pageIndex,
            int? singletonPageCount = null)
        {
            var classInfo = _classRepository.GetAllIncluding(x => x.Admins).FirstOrDefault(x => x.Id == classId);
            if (classInfo == null)
            {
                return new PaginationDataList<AdminDto>();
            }
            var list = classInfo.Admins
                .AsQueryable()
                .ToPagination(pageIndex, singletonPageCount);
            return ObjectMapper.Map<PaginationDataList<AdminDto>>(list);
        }

        public bool AllocationStudentClass(Guid studentId, int classId)
        {
            var classInfo = _classRepository.FirstOrDefault(classId);
            if (classInfo == null)
            {
                throw new UserFriendlyException("The class does not exist");
            }
            var student = StudentRepository.FirstOrDefault(x => x.StudentId == studentId);
            if (student == null)
            {
                throw new UserFriendlyException("The student does not exist");
            }
            return StudentRepository.Update(student).Id > 0;
        }

        public PaginationDataList<ClassInfoOut> GetInstitutionAllClasses(
            int institutionId,
            int? pageIndex,
            int? singletonPageCount = null)
        {
            var list = _classRepository
                .GetAllList(x => x.InstitutionId == institutionId).AsQueryable()
                .ToPagination(pageIndex, singletonPageCount);
            return ObjectMapper.Map<PaginationDataList<ClassInfoOut>>(list);
        }
    }
}
