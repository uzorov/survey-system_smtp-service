using MassMailService.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MassMailService.Services
{
    public class ClientRepository
    {
        private readonly string _connectionString;
        public ClientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            var clients = new List<Client>();
            Console.WriteLine($"[DB] Попытка подключения к базе данных: {_connectionString}");
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();
                Console.WriteLine("[DB] Соединение с БД установлено успешно.");
                using var cmd = new NpgsqlCommand("SELECT id, email, name FROM clients", conn);
                Console.WriteLine("[DB] Выполнение SQL-запроса...");
using var reader = await cmd.ExecuteReaderAsync();
Console.WriteLine("[DB] Запрос выполнен, чтение данных...");
                while (await reader.ReadAsync())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Name = reader.GetString(2)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB][ERROR] Ошибка подключения или запроса: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[DB][ERROR] Внутренняя ошибка: {ex.InnerException.Message}");
            }
            return clients;
        }
    }
}
