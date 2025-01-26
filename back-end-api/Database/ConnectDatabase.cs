using System;
using Npgsql;
using dotenv.net;

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

			//Console.WriteLine(this.connectionString);
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
			if(this.connection == null)
			{
				return;
			}

			this.connection.Close();
			this.connection.Dispose();
			this.connection = null;

			Console.WriteLine("Database connection closed");
		}

		public void Query (string sql)
		{
			try
			{
				this.ConnectTo();

				NpgsqlCommand cmd = new NpgsqlCommand(sql, this.connection);
				NpgsqlDataReader reader = cmd.ExecuteReader();

				while(reader.Read())
				{
					//var val = reader.GetValue(reader.GetOrdinal(0));
					Console.WriteLine(reader.FieldCount);
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
		}

		public void Exec (string sql)
		{

		}
	}
}