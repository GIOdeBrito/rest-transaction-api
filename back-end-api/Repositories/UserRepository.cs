using BackEndApi.Interfaces;
using BackEndApi.DTO.User;
using BackEndApi.Database;

namespace BackEndApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PostgresDatabase _db;

    public UserRepository(PostgresDatabase db)
    {
        _db = db;
    }

    public async Task<bool> CreateAsync(UserCreateForm user, string hashedPassword)
    {
        return await _db.ExecuteAsync(@"
            INSERT INTO users (name, fullname, secret, mail, createdat, role)
            VALUES (@Name, @Fullname, @Secret, @Mail, CURRENT_DATE, 'User')",
            new
            {
                Name     = user.Name,
                Fullname = $"{user.Name} {user.Surname}",
                Secret   = hashedPassword,
                Mail     = user.Mail
            });
    }

	public async Task<bool> IsUserByNameAsync (string name)
	{
		var rows = await _db.QueryAsync<User>(
            "SELECT * FROM users WHERE name = @Name",
            new { Name = name }
        );

		return rows.Length > 0;
	}

	public async Task<User?> GetUserByNameAsync (string name)
	{
		var rows = await _db.QueryAsync<User>(
			"SELECT * FROM users WHERE name = @Name LIMIT 1",
			new { Name = name }
		);

		return rows.Length > 0 ? rows[0] : null;
	}
}