using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.DTOs.CheckConnectionDTOs;
using Study1CApi.Models;

namespace Study1CApi.Interfaces
{
    public interface IConnectionRepository
    {
        public Task<CheckConnectionDTO> CheckConnectionAsync();

        public Study1cDbContext CreateNewContext();
    }
}
