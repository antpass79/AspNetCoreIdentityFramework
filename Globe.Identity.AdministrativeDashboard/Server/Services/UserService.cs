using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser, string> _userRepository;
        private readonly IRepository<ApplicationRole, string> _roleRepository;

        public UserService(IMapper mapper, IRepository<ApplicationUser, string> userRepository, IRepository<ApplicationRole, string> roleRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public void Delete(ApplicationUserDTO entity)
        {
            var mappedUser = _mapper.Map<ApplicationUser>(entity);

            _userRepository.Delete(mappedUser);
        }

        public void Delete(string id)
        {
            var user = _userRepository.FindById(id);
            var mappedUser = _mapper.Map<ApplicationUser>(user);
            _userRepository.Delete(mappedUser);
        }

        public UserWithRoles FindById(string id)
        {
            var user =_userRepository.FindById(id);
            var userRoles = user.Roles.Select(role => role.RoleId);

            return new UserWithRoles
            {
                User = _mapper.Map<ApplicationUserDTO>(user),
                Roles = _mapper.Map<IEnumerable<ApplicationRoleDTO>>(_roleRepository.Get().Where(role => userRoles.Contains(role.Id)))
            };
        }

        public IEnumerable<ApplicationUserDTO> Get(Expression<Func<ApplicationUserDTO, bool>> filter = null, Func<IQueryable<ApplicationUserDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null)
        {
            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(_userRepository.Get());
        }

        public void Insert(UserWithRoles entity)
        {
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

            _userRepository.Insert(mappedUser);
        }

        public void Update(UserWithRoles entity)
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

            _userRepository.Update(mappedUser);
        }
    }
}
