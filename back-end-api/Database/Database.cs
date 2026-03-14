using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Npgsql;

namespace BackEndApi.Database
{
    public class PostgresDatabase : IDisposable
    {
        private readonly NpgsqlDataSource _dataSource;

        public PostgresDatabase()
        {
            var connString = BuildConnectionString();
            _dataSource = NpgsqlDataSource.Create(connString);
            // Optional: Console.WriteLine(connString);
        }

        private static string BuildConnectionString()
        {
            var user   = Environment.GetEnvironmentVariable("POSTGRES_LOGIN")   ?? throw new InvalidOperationException("POSTGRES_LOGIN missing");
            var pass   = Environment.GetEnvironmentVariable("POSTGRES_PASSWD") ?? throw new InvalidOperationException("POSTGRES_PASSWD missing");
            var db     = Environment.GetEnvironmentVariable("POSTGRES_DB")     ?? throw new InvalidOperationException("POSTGRES_DB missing");
            var port   = Environment.GetEnvironmentVariable("POSTGRES_PORT")   ?? "5432";
            var host   = Environment.GetEnvironmentVariable("POSTGRES_HOST")   ?? "localhost";

            return $"Host={host};Port={port};Database={db};Username={user};Password={pass};Pooling=true;Minimum Pool Size=0;Maximum Pool Size=100;";
        }

        public async Task<T[]> QueryAsync<T>(string sql, object? parameters = null) where T : new()
        {
            var items = new List<T>();

            await using var conn = await _dataSource.OpenConnectionAsync();
            await using var cmd  = conn.CreateCommand();
            cmd.CommandText = sql;

            if(parameters != null)
            {
                var paramDict = ExtractParameters(parameters);
                foreach (var kv in paramDict)
                {
                    cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
                }
            }

            await using var reader = await cmd.ExecuteReaderAsync();

            while(await reader.ReadAsync())
            {
                var row = new T();

                for(int i = 0; i < reader.FieldCount; i++)
                {
                    string colName = reader.GetName(i);

                    var prop = typeof(T).GetProperty(colName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    var field = prop == null
                        ? typeof(T).GetField(colName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        : null;

                    var member = (MemberInfo?)prop ?? field;
                    if (member == null) continue;

                    object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                    if (member is PropertyInfo p) p.SetValue(row, value);
                    else if (member is FieldInfo f)  f.SetValue(row, value);
                }

                items.Add(row);
            }

            return items.ToArray();
        }

        public async Task<bool> ExecuteAsync(string sql, object? parameters = null)
        {
            await using var conn = await _dataSource.OpenConnectionAsync();
            await using var cmd  = conn.CreateCommand();
            cmd.CommandText = sql;

            if(parameters != null)
            {
                var paramDict = ExtractParameters(parameters);
                foreach (var kv in paramDict)
                {
                    cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
                }
            }

            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        private static Dictionary<string, object?> ExtractParameters(object obj)
        {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in obj.GetType().GetProperties())
            {
                dict[prop.Name] = prop.GetValue(obj);
            }

            return dict;
        }

        public void Dispose()
        {
            _dataSource.Dispose();
        }
    }
}