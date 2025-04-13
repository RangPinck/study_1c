using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Study1CApi.DTOs.MaterialDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;

namespace Study1CApi.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly Study1cDbContext _context;

        public MaterialRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaterialTypeDTO>> GetMaterialsTypesAsync()
        {
            return await _context.MaterialTypes.AsNoTracking().Select(material => new MaterialTypeDTO()
            {
                TypeId = material.TypeId,
                TypeName = material.TypeName
            }).ToListAsync();
        }
    }
}
