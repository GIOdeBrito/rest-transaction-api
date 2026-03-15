using BackEndApi.DTO.User;

namespace BackEndApi.Interfaces;

public interface IUserRepository
{
    Task<bool> CreateAsync(UserCreateForm user, string hashedPassword);
	Task<bool> IsUserByNameAsync (string name);
	Task<User?> GetUserByNameAsync (string name);
}