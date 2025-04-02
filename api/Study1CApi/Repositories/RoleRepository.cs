using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.RoleDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;

namespace Study1CApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly Study1cDbContext _context;

        public RoleRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<RoleDTO>> GetAllRoles()
        {
            return await _context.Roles.Select(role => new RoleDTO()
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
            }).ToListAsync();
        }

        public async Task<bool> AddRole(NewRoleDTO role)
        {
            var newRole = new Role()
            {
                RoleName = role.RoleName
            };

            await _context.Roles.AddAsync(newRole);

            return await SaveChanges();
        }

        public async Task<bool> UpdateRole(RoleDTO role)
        {
            var upRole = await _context.Roles.FirstAsync(x => x.RoleId == role.RoleId);
            upRole.RoleName = role.RoleName;
            _context.Roles.Update(upRole);
            return await SaveChanges();
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            var delRole = await _context.Roles.FirstAsync(x => x.RoleId == roleId);
            _context.Roles.Remove(delRole);
            return await SaveChanges();
        }

        public async Task<bool> RoleIsExistOfId(int roleId)
        {
            return await _context.Roles.AnyAsync(x => x.RoleId == roleId);
        }

        public async Task<RoleDTO?> GetRoleById(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId);
            return role != null ? new RoleDTO() { RoleId = role.RoleId, RoleName = role.RoleName } : null;
        }

        public async Task<bool> RoleIsExistOfName(string roleName)
        {
            return await _context.Roles.AnyAsync(x => x.RoleName == roleName);
        }

        public async Task<bool> SaveChanges()
        {
            var save = await _context.SaveChangesAsync();
            return save > 0;
        }
    }
}
