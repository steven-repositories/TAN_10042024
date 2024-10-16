﻿using Microsoft.EntityFrameworkCore;
using TAN_10042024.Application.Abstractions.Queries;
using TAN_10042024.Domain.Entities;

namespace TAN_10042024.Infrastructure.Data.Queries {
    public class ClientQueryService : IClientQueryService {
        private readonly ILogger<ClientQueryService> _logger;
        private readonly AppDbContext _dbContext;

        public ClientQueryService(ILogger<ClientQueryService> logger, AppDbContext dbContext) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Task<Client?> GetClientByName(string clientName) {
            return _dbContext
                .Set<Client>()
                .Where(client => client.Name == clientName)
                .FirstOrDefaultAsync();
        }
    }
}
