using System.Collections.Generic;
using Abp.Application.Services;
using Imagine.BookManager.Dto.Set;

namespace Imagine.BookManager.SetService
{
    public interface ISetAppService: IApplicationService
    {
        int CreateSet(SetDto set);

        SetDto GetSetById(int id);

        bool CheckSetName(string name);

        List<SetDto> GetAllSet();
    }
}