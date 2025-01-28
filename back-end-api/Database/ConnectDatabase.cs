using System;
using System.Reflection;
using Npgsql;
using dotenv.net;
using BackEndApi.Models;

namespace BackEndApi.Database
{
	public class DatabaseConnection
	{
		private string connectionString = "";
		private NpgsqlConnection? connection = null;

		public DatabaseConnection ()
		{
			string dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
			string dbSecret = Environment.GetEnvironmentVariable("DATABASE_SCRT");
			string dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
			string dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT");
			string dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");

			this.connectionString = $"Server=app;Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbSecret}";
		}

		~DatabaseConnection ()
		{
			this.Close();
		}

		private void ConnectTo ()
		{
			try
			{
				this.connection = new NpgsqlConnection(this.connectionString);
				connection.Open();

				Console.WriteLine("Database connection open");
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Could not open connection: {ex}");
			}
		}

		private void Close ()
		{
			if(this.connection is null)
			{
				return;
			}

			this.connection.Close();
			this.connection.Dispose();
			this.connection = null;

			Console.WriteLine("Database connection closed");
		}

		public List<T> Query<T>(string sql) where T : new()
		{
			List<T> items = new();

			try
			{
				this.ConnectTo();

				using(NpgsqlCommand cmd = new NpgsqlCommand(sql, this.connection))
				using(NpgsqlDataReader reader = cmd.ExecuteReader())
				{
					while(reader.Read())
					{
						T row = new T();

						for(int i = 0; i < reader.FieldCount; i++)
						{
							// Columns are always strings
							string columnName = reader.GetName(i);

							FieldInfo field = typeof(T).GetField(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
							//PropertyInfo property = typeof(T).GetProperty(columnName);

							if(field is null)
							{
								continue;
							}

							// The type of the field varies
							Type columnType = reader.GetFieldType(i);
							dynamic rowValue = null;

							if(columnType == typeof(int))
							{
								rowValue = reader.GetInt32(i);
							}

							if(columnType == typeof(string))
							{
								rowValue = reader.GetString(i);
							}

							field.SetValue(row, rowValue);
							//property.SetValue(row, rowValue);
						}

						items.Add(row);
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Could not perform query: {ex}");
			}
			finally
			{
				this.Close();
			}

			return items;
		}
	}
}