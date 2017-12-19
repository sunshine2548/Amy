using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Set;
using System.Collections.Generic;
using System.Linq;

namespace Imagine.BookManager.SetService
{
    public class SetAppService : BookManagerAppServiceBase, ISetAppService
    {
        private readonly IRepository<Set> _setRepostitory;

        public SetAppService(IRepository<Set> setRepository)
        {
            _setRepostitory = setRepository;
        }

        public bool CheckSetName(string name)
        {
            var tempSet = _setRepostitory.FirstOrDefault(x => x.SetName == name);
            if (tempSet != null)
            {
                return false;
            }
            return true;
        }

        public int CreateSet(SetDto set)
        {
            var tempSet = _setRepostitory.FirstOrDefault(x => x.SetName == set.SetName);
            if (tempSet != null)
            {
                throw new UserFriendlyException("The name already exists");
            }
            Set setEntity = ObjectMapper.Map<Set>(set);
            return _setRepostitory.InsertAndGetId(setEntity);
        }

        public SetDto GetSetById(int id)
        {
            var tempSet = _setRepostitory.GetAllIncluding(x => x.Books).FirstOrDefault(x => x.Id == id);
            if (tempSet == null)
                return new SetDto();
            return ObjectMapper.Map<SetDto>(tempSet);
        }

        public List<SetDto> GetAllSet()
        {
            var list = _setRepostitory.GetAllIncluding(x => x.Books).ToList();
            return ObjectMapper.Map<List<SetDto>>(list);
        }
    }
}