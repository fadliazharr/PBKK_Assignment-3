using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using People.Models;

namespace People
{
    public class PersonRepository
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public string StatusMessage { get; set; }

        public PersonRepository(string dbPath)
        {
            _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));
        }

        private async Task EnsureConnectionInitializedAsync()
        {
            if (_connection == null)
            {
                _connection = new SQLiteAsyncConnection(_dbPath);
                await _connection.CreateTableAsync<Person>();
            }
        }

        public async Task AddNewPersonAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }

            await EnsureConnectionInitializedAsync();

            var person = new Person { Name = name };
            var result = await _connection.InsertAsync(person);
            StatusMessage = $"{result} record(s) added [Name: {name}]";
        }

        public async Task<IReadOnlyList<Person>> GetAllPeopleAsync()
        {
            await EnsureConnectionInitializedAsync();

            try
            {
                var people = await _connection.Table<Person>().ToListAsync();
                return people;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to retrieve data. {ex.Message}";
                return new List<Person>().AsReadOnly();
            }
        }
    }
}
