using System;
using System.Reflection;
using Npgsql;
using dotenv.net;
using BackEndApi.Models;

namespace BackEndApi.Database
{
	public class PostgresDatabase
	{
		private string connectionString = "";
		private NpgsqlConnection? connection = null;
		private NpgsqlCommand? cmd = null;

		public PostgresDatabase ()
		{
			string dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
			string dbSecret = Environment.GetEnvironmentVariable("DATABASE_SCRT");
			string dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
			string dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT");
			string dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");

			this.connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbSecret}";
		}

		~PostgresDatabase ()
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

			ClearCommand();

			Console.WriteLine("Database connection closed");
		}

		private void ClearCommand ()
		{
			if(this.cmd is null)
			{
				return;
			}

			//this.cmd.Close();
			this.cmd.Dispose();
			this.cmd = null;
		}

		public T[] Query<T>(string sql, object? sqlParams = null) where T : new()
		{
			List<T> items = new();

			try
			{
				this.ConnectTo();

				cmd = new NpgsqlCommand(sql, this.connection);

				if(sqlParams is not null)
				{
					Dictionary<string, dynamic> acquiredParams = ExtractParameters(sqlParams);

					// Bind query params
					foreach(KeyValuePair<string, dynamic> kvp in acquiredParams)
					{
						//Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
						cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
					}
				}

				using(NpgsqlDataReader reader = cmd.ExecuteReader())
				{
					while(reader.Read())
					{
						T row = new T();

						for(int i = 0; i < reader.FieldCount; i++)
						{
							// Columns are always strings
							string columnName = reader.GetName(i);

							FieldInfo? field = typeof(T).GetField(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
							PropertyInfo? property = null;

							if(field is null)
							{
								// If no field was found, looks for a property instead
								property = typeof(T).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
							}

							// If no property or field was found, skip this iteration
							if(field is null && property is null)
							{
								continue;
							}

							// The type of the field varies
							Type columnType = reader.GetFieldType(i);
							dynamic? rowValue = null;

							if(columnType == typeof(int))
							{
								rowValue = reader.GetInt32(i);
							}

							if(columnType == typeof(string))
							{
								rowValue = reader.GetString(i);
							}

							//Console.WriteLine($"Row: {row} Value: {rowValue}");

							field?.SetValue(row, rowValue);
							property?.SetValue(row, rowValue);
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

			return items.ToArray();
		}

		private Dictionary<string, dynamic> ExtractParameters (object sqlparams)
		{
			Dictionary<string, dynamic> list = new ();

			foreach(var property in sqlparams.GetType().GetProperties())
	        {
				list.Add(property.Name, property.GetValue(sqlparams));
	        }

			return list;
		}
	}
}