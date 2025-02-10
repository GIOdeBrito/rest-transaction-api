namespace BackEndApi.Models.User
{
	public class UserCreateForm
	{
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Secret { get; set; }
		public string? Mail { get; set; }
	}
}