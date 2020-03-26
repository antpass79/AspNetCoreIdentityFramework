using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class UserService : IAsyncUserService
    {
        private readonly IMapper _mapper;
        private readonly IAsyncUserUnitOfWork _userUnitOfWork;
        private readonly IAsyncRoleRepository _roleRepository;

        public UserService(IMapper mapper, IAsyncUserUnitOfWork userUnitOfWork, IAsyncRoleRepository roleRepository)
        {
            _mapper = mapper;
            _userUnitOfWork = userUnitOfWork;
            _roleRepository = roleRepository;
        }

        async public Task DeleteAsync(ApplicationUserDTO entity)
        {
            var mappedUser = _mapper.Map<ApplicationUser>(entity);

            await _userUnitOfWork.UserRepository.DeleteAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }

        async public Task DeleteAsync(string id)
        {
            var user = await _userUnitOfWork.UserRepository.FindByIdAsync(id);
            var mappedUser = _mapper.Map<ApplicationUser>(user);

            await _userUnitOfWork.UserRepository.DeleteAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }

        async public Task<UserWithRoles> FindByIdAsync(string id)
        {
            var user = await _userUnitOfWork.UserRepository.FindByIdAsync(id);
            var userRoles = user.Roles.Select(role => role.RoleId);

            var allRoles = await _roleRepository.GetAsync();

            return new UserWithRoles
            {
                User = _mapper.Map<ApplicationUserDTO>(user),
                Roles = _mapper.Map<IEnumerable<ApplicationRoleDTO>>(allRoles.Where(role => userRoles.Contains(role.Id)))
            };
        }

        async public Task<IEnumerable<ApplicationUserDTO>> GetAsync(Expression<Func<ApplicationUserDTO, bool>> filter = null, Func<IQueryable<ApplicationUserDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null)
        {
            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(await _userUnitOfWork.UserRepository.GetAsync());
        }

        async public Task InsertAsync(UserWithRoles entity)
        {
            entity.User.Id = Guid.NewGuid().ToString();

            var mappedUser = _mapper.Map<ApplicationUser>(entity.User);
            var mappedRoles = _mapper.Map<IEnumerable<ApplicationRole>>(entity.Roles);

            var roles = mappedRoles.Select(role => role.Id);
            foreach (var role in roles)
            {
                mappedUser.Roles.Add(new IdentityUserRole<string>
                {
                    UserId = entity.User.Id,
                    RoleId = role
                });
            }

            await _userUnitOfWork.UserRepository.InsertAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }

        async public Task UpdateAsync(UserWithRoles entity)
        {
            var mappedUser = _mapper.Map<ApplicationUser>(entity.User);
            var mappedRoles = _mapper.Map<IEnumerable<ApplicationRole>>(entity.Roles);

            var roles = mappedRoles.Select(role => role.Id);
            var userRoles = mappedUser.Roles.Select(role => role.RoleId);

            foreach (string role in userRoles.Except(roles))
            {
                var identityRole = mappedUser.Roles.ToList().Find(item => item.RoleId == role);
                mappedUser.Roles.Remove(identityRole);
            }

            foreach (var role in roles.Except(userRoles))
            {
                mappedUser.Roles.Add(new IdentityUserRole<string>
                {
                    UserId = entity.User.Id,
                    RoleId = role
                });
            }

            await _userUnitOfWork.UserRepository.UpdateAsync(mappedUser);
            await _userUnitOfWork.SaveAsync();
        }
    }
}
