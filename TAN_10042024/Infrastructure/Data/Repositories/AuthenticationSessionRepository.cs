﻿using TAN_10042024.Application.Abstractions.Repositories;
using TAN_10042024.Application.Builders;
using TAN_10042024.Application.Utilities;
using TAN_10042024.Domain.Entities;
using static TAN_10042024.Application.Utilities.Exceptions;

namespace TAN_10042024.Infrastructure.Data.Repositories {
    public class AuthenticationSessionRepository : IAuthenticationSessionRepository {
        private readonly ILogger<AuthenticationSessionRepository> _logger;
        private readonly AppDbContext _dbContext;

        public AuthenticationSessionRepository(ILogger<AuthenticationSessionRepository> logger, AppDbContext dbContext) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task SaveAuthKey(Guid key, string clientName) {
            _logger.LogInformation("Saving to database the auth key generated for client {0}."
            .FormatWith(clientName));

            try {
                var client = _dbContext
                    .Set<Client>()
                    .Where(client => client.Name == clientName)
                    .FirstOrDefault()!;

                var newAuthSession = AuthenticationSessionBuilder
                    .Initialize()
                    .WithKey(key)
                    .WithClient(client)
                    .Build();

                await _dbContext
                    .Set<AuthenticationSession>()
                    .AddAsync(newAuthSession);

                var authId = await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Auth key is saved to database with key of: {0}."
                    .FormatWith(authId));
            } catch (Exception e) {
                var errorMessage = "Error encountered when saving auth key: {0}"
                    .FormatWith(e.Message);

                _logger.LogError(errorMessage);
                throw new RepositoryException(errorMessage, e);
            }
        }
    }
}
