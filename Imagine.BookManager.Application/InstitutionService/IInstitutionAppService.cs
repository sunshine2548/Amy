using Abp.Application.Services;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.StudentService;
using System.Threading.Tasks;
using System.Web.Http;
using Imagine.BookManager.Dto.Institution;
using Imagine.BookManager.Dto.Student;

namespace Imagine.BookManager.InstitutionService
{
    public interface IInstitutionAppService : IApplicationService
    {
        [HttpPost]
        int CreateInstitution(Institution institution);
        [HttpPost]
        Task<int> CreateInstitutionAsync(Institution institution);
        [HttpGet]
        Institution GetInstitution(int id);
        [HttpGet]
        Task<InstitutionOut> GetInstitutionAsync(int id);
        [HttpGet]
        PaginationDataList<InstitutionOut> GetAll(int? pageIndex, int? singletonPageCount = null);
        [HttpGet]
        Institution GetInstitutionByName(string name);
        [HttpGet]
        bool CheckInstitutionName(string name);

        [HttpGet]
        PaginationDataList<StudentOut> GetAllInstitutionIdStudent(
            int institutionId,
            int? pageIndex,
            int? singletonPageCount = null);

        [HttpPost]
        bool Update(InstitutionOut institution);
    }
}
