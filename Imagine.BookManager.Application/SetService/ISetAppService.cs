using System;
using System.Collections.Generic;
using System.Web.Http;
using Abp.Application.Services;
using Imagine.BookManager.Common;
using Imagine.BookManager.Dto.Set;

namespace Imagine.BookManager.SetService
{
    public interface ISetAppService: IApplicationService
    {
        [HttpPost]
        int CreateSet(SetDto set);

        [HttpGet]
        SetDto GetSetById(int id);

        [HttpPost]
        bool CheckSetName(string name);

        [HttpGet]
        List<SetDto> GetAllSet();

        [HttpGet]
        PaginationDataList<SetDto> SearchPicBook(int? pageSize, int? pageRows, int setStatus, Guid userId);
    }
}