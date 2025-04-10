using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Study1CApi.DTOs.CheckConnectionDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;

namespace Study1CApi.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly Study1cDbContext _context;

        public ConnectionRepository(Study1cDbContext context)
        {
            _context = context;
        }

        public async Task<CheckConnectionDTO> CheckConnectionAsyncAsync()
        {
            using (var connection = new Study1cDbContext())
            {
                var connect = new CheckConnectionDTO();

                try
                {
                    connect.IsConnect = (ConnectionEnum)Convert.ToInt32(await connection.Database.CanConnectAsync());
                    return connect;
                }
                catch (Exception ex)
                {
                    connect.IsConnect = ConnectionEnum.NoConnectBD;
                    connect.Error = ex.Message;
                    return connect;
                }
            }
        }

        public Study1cDbContext CreateNewContext()
        {
            return new Study1cDbContext();
        }
    }
}
