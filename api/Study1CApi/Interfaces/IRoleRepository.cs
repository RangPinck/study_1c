using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.DTOs;
using Study1CApi.DTOs.RoleDTOs;

namespace Study1CApi.Interfaces
{
    public interface IRoleRepository
    {
        public Task<ICollection<RoleDTO>> GetAllRoles();

        public Task<bool> AddRole(NewRoleDTO role);

        public Task<bool> UpdateRole(RoleDTO role);

        public Task<bool> DeleteRole(int roleId);

        public Task<RoleDTO?> GetRoleById(int roleId);

        public Task<bool> RoleIsExistOfId(int roleId);

        public Task<bool> RoleIsExistOfName(string roleName);

        public Task<bool> SaveChanges();
    }
}
