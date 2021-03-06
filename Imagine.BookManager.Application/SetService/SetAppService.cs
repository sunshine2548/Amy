﻿using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Set;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagine.BookManager.SetService
{
    public class SetAppService : BookManagerAppServiceBase, ISetAppService
    {
        private readonly IRepository<Set> _setRepostitory;

        private readonly IRepository<Admin> _adminRepostitory;

        private readonly IRepository<Book> _bookRepostitory;

        public SetAppService(IRepository<Set> setRepository, IRepository<Admin> adminRepostitory, IRepository<Book> bookRepostitory)
        {
            _setRepostitory = setRepository;
            _adminRepostitory = adminRepostitory;
            _bookRepostitory = bookRepostitory;
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
                throw new UserFriendlyException(ExceptionInfo.SetNameExists);
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

        public PaginationDataList<SetDto> SearchPicBook(int? pageSize, int? pageRows, int setStatus, Guid userId)
        {
            var admin = _adminRepostitory.GetAllIncluding(
                e => e.Orders.Select(or => or.OrderItems.Select(oi => oi.TeacherAllocations)),
                e => e.Orders.Select(or => or.Payments),
                e => e.TeacherAllocations.Select(ta => ta.StudentAllocations)
            ).FirstOrDefault(e => e.UserId == userId);
            if (admin.UserType == UserType.Admin)
            {
                return GetPicBooksByAdmin(pageSize, pageRows, setStatus, admin);
            }
            else
            {
                return GetPicBooksByTeacher(pageSize, pageRows, setStatus, admin);
            }
        }

        private PaginationDataList<SetDto> GetPicBooksByAdmin(int? pageSize, int? pageRows, int setStatus, Admin admin)
        {
            List<SetDto> setDtoList = new List<SetDto>();
            var orderItems = admin.Orders.SelectMany(e => e.OrderItems);
            var payments = admin.Orders.SelectMany(e => e.Payments);
            foreach (var orderItem in orderItems)
            {
                var allocatedNum = orderItem.TeacherAllocations.Sum(e => e.Credit);
                if (FilterSetBySetStatus(setStatus, allocatedNum, orderItem.Quantity))
                    continue;
                SetDto setDto = new SetDto()
                {
                    Id = orderItem.SetId,
                    Synopsis = orderItem.Set.Synopsis,
                    SetName = orderItem.Set.SetName,
                    SetType = HintInfo.StandardCourse,
                    Books = _bookRepostitory.GetAllList(e => e.SetId == orderItem.SetId),
                    DateCreated = payments.FirstOrDefault(e => e.OrderRef == orderItem.OrderRef)?.DateCreated,
                    SurplusCredit = allocatedNum + "/" + orderItem.Quantity
                };
                setDtoList.Add(setDto);
            }
            return setDtoList.OrderByDescending(e => e.DateCreated).AsQueryable().ToPagination(pageSize, pageRows);
        }

        private PaginationDataList<SetDto> GetPicBooksByTeacher(int? pageSize, int? pageRows, int setStatus, Admin admin)
        {
            List<SetDto> setDtoList = new List<SetDto>();
            foreach (var teacherAllocation in admin.TeacherAllocations)
            {
                int allocatedNum = teacherAllocation.StudentAllocations.Count;
                if (FilterSetBySetStatus(setStatus, allocatedNum, teacherAllocation.Credit))
                    continue;
                var setInfo = _setRepostitory.GetAllIncluding(e => e.Books).FirstOrDefault(e => e.Id == teacherAllocation.SetId);
                SetDto setDto = new SetDto()
                {
                    Id = teacherAllocation.SetId,
                    SetName = setInfo.SetName,
                    SetType = HintInfo.StandardCourse,
                    Synopsis = setInfo.Synopsis,
                    Books = setInfo.Books,
                    DateCreated = teacherAllocation.DateAllocated,
                    SurplusCredit = allocatedNum + "/" + teacherAllocation.Credit
                };
                setDtoList.Add(setDto);
            }
            return setDtoList.OrderByDescending(e => e.DateCreated).AsQueryable().ToPagination(pageSize, pageRows);
        }

        private bool FilterSetBySetStatus(int setStatus, int allocatedNum, int quantity)
        {
            switch (setStatus)
            {
                case 1:
                    return allocatedNum == 0;
                case 2:
                    return allocatedNum > 0;
                case 3:
                    return allocatedNum != quantity;
                default:
                    return false;
            }
        }
    }
}