using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.DTOs.MaterialDTOs;

namespace Study1CApi.Interfaces
{
    public interface IMaterialRepository
    {
        public Task<IEnumerable<MaterialTypeDTO>> GetMaterialsTypesAsync();
    }
}
