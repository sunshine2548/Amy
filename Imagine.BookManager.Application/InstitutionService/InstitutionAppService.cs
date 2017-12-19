using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Institution;
using Imagine.BookManager.Dto.Student;
using System.Linq;
using System.Threading.Tasks;

namespace Imagine.BookManager.InstitutionService
{
    public class InstitutionAppService : BookManagerAppServiceBase, IInstitutionAppService
    {
        private readonly IRepository<Institution> _institutionRepository;

        public IRepository<ClassInfo> ClassInfoRepository { get; set; }

        public InstitutionAppService(IRepository<Institution> institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public int CreateInstitution(Institution institution)
        {
            var tempInstitution = _institutionRepository.FirstOrDefault(x => x.Name == institution.Name);
            if (tempInstitution != null)
            {
                throw new UserFriendlyException("The institution already exists");
            }
            return _institutionRepository.InsertAndGetId(institution);
        }

        public async Task<int> CreateInstitutionAsync(Institution institution)
        {
            var tempInstitution = _institutionRepository.FirstOrDefault(x => x.Name == institution.Name);
            if (tempInstitution != null)
            {
                throw new UserFriendlyException("The institution already exists");
            }
            return await _institutionRepository.InsertAndGetIdAsync(institution);

        }

        public Institution GetInstitution(int id)
        {
            return _institutionRepository.FirstOrDefault(id);
        }

        public async Task<InstitutionOut> GetInstitutionAsync(int id)
        {
            var task = await _institutionRepository.FirstOrDefaultAsync(id);
            return ObjectMapper.Map<InstitutionOut>(task);
        }

        public PaginationDataList<InstitutionOut> GetAll(int? pageIndex, int? singletonPageCount)
        {
            var list = _institutionRepository.GetAll()
                .OrderByDescending(x => x.Id)
                .ToPagination(pageIndex, singletonPageCount);
            return ObjectMapper.Map<PaginationDataList<InstitutionOut>>(list);
        }

        public Institution GetInstitutionByName(string name)
        {
            Institution institution = _institutionRepository.FirstOrDefault(x => x.Name == name);
            if (institution != null)
                return institution;
            return new Institution();
        }

        public bool CheckInstitutionName(string name)
        {
            var institution = _institutionRepository.FirstOrDefault(x => x.Name == name);
            if (institution == null)
                return true;
            return false;
        }

        public PaginationDataList<StudentOut> GetAllInstitutionIdStudent(
            int institutionId,
            int? pageIndex,
            int? singletonPageCount)
        {
            var classInfoList = ClassInfoRepository
                .GetAllIncluding(x => x.Students)
                .Where(x => x.InstitutionId == institutionId);
            var allStudents = classInfoList.SelectMany(c => c.Students);
            var listResult = from s in allStudents
                             join c in classInfoList on (s.ClassId.HasValue ? s.ClassId.Value : 0) equals c.Id into temp
                             from t in temp.DefaultIfEmpty()
                             select new StudentOut
                             {
                                 ClassId = s.ClassId,
                                 ClassName = t == null ? "" : t.Name,
                                 DateCreated = s.DateCreated,
                                 DateOfBirth = s.DateOfBirth,
                                 FullName = s.FullName,
                                 Gender = s.Gender ? 1 : 0,
                                 GuardianName = s.GuardianName,
                                 Id = s.Id,
                                 IsDelete = s.IsDelete ? 1 : 0,
                                 Mobile = s.Mobile,
                                 Picture = s.Picture,
                                 StudentId = s.StudentId,
                                 UserName = s.UserName
                             };
            return ObjectMapper.Map<PaginationDataList<StudentOut>>(
                listResult.AsQueryable().OrderBy(x => x.Id)
                .ToPagination(pageIndex, singletonPageCount)
             );
        }

        public bool Update(InstitutionOut institution)
        {
            var temp = _institutionRepository.FirstOrDefault(institution.Id);
            if (temp == null)
            {
                throw new UserFriendlyException("The institution not exsists");
            }
            ObjectMapper.Map(institution, temp);
            return _institutionRepository.Update(temp) != null;
        }
    }
}
