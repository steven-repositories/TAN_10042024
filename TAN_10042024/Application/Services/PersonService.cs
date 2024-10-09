﻿using TAN_10042024.Application.Abstractions.Controllers;
using TAN_10042024.Infrastructure.Data.Queries;
using TAN_10042024.Infrastructure.Data.Repositories;

namespace TAN_10042024.Application.Services
{
    public class PersonService : IPersonService {
        private readonly ILogger<PersonService> _logger;
        private readonly PersonsRepository _personsRepo;
        private readonly PersonsQueryService _personsQueryService;

        public PersonService(ILogger<PersonService> logger, PersonsRepository personsRepo, PersonsQueryService personsQueryService) {
            _logger = logger;
            _personsRepo = personsRepo;
            _personsQueryService = personsQueryService;
        }

        public async Task<int> GetMaxScoreByTeam(string team) {
            var persons = await _personsQueryService.GetPersonsByTeam(team);

            return persons.Max(person => person.Score);
        }

        public async Task<int> GetSecondToLeastScoreByTeam(string team) {
            var persons = await _personsQueryService.GetPersonsByTeam(team);

            var secondToLeastScore = persons
                .OrderBy(person => person.Score)
                .Skip(1)
                .Select(person => person.Score)
                .FirstOrDefault();

            return secondToLeastScore;
        }

        public async Task<string> UnionizePersonNamesByTeam(string team) {
            var persons = await _personsQueryService.GetPersonsByTeam(team);
            var personNames = persons
                .Select(person => person.Name)
                .ToList();

            return string.Join(", ", personNames);
        }
    }
}
