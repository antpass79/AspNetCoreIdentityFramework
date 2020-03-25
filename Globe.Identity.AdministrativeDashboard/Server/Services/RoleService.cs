using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRoleUnitOfWork _roleUnitOfWork;

        public RoleService(IMapper mapper, IRoleUnitOfWork roleUnitOfWork)
        {
            _mapper = mapper;
            _roleUnitOfWork = roleUnitOfWork;
        }

        public void Delete(ApplicationRoleDTO entity)
        {
            var mappedRole = _mapper.Map<ApplicationRole>(entity);

            _roleUnitOfWork.RoleRepository.Delete(mappedRole);
            _roleUnitOfWork.Save();
        }

        public void Delete(string id)
        {
            var role = _roleUnitOfWork.RoleRepository.FindById(id);
            var mappedRole = _mapper.Map<ApplicationRole>(role);

            _roleUnitOfWork.RoleRepository.Delete(mappedRole);
            _roleUnitOfWork.Save();
        }

        public ApplicationRoleDTO FindById(string id)
        {
            var role = _roleUnitOfWork.RoleRepository.FindById(id);
            return _mapper.Map<ApplicationRoleDTO>(role);
        }

        public IEnumerable<ApplicationRoleDTO> Get(Expression<Func<ApplicationRoleDTO, bool>> filter = null, Func<IQueryable<ApplicationRoleDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null)
        {
            return _mapper.Map<IEnumerable<ApplicationRoleDTO>>(_roleUnitOfWork.RoleRepository.Get());
        }

        public void Insert(ApplicationRoleDTO entity)
        {
            var mappedRole = _mapper.Map<ApplicationRole>(entity);

            _roleUnitOfWork.RoleRepository.Insert(mappedRole);
            _roleUnitOfWork.Save();
        }

        public void Update(ApplicationRoleDTO entity)
        {
            var mappedRole = _mapper.Map<ApplicationRole>(entity);

            _roleUnitOfWork.RoleRepository.Update(mappedRole);
            _roleUnitOfWork.Save();
        }
    }
}
