namespace BackEndApi.Models
{
	public class User
	{
		public int? Id;
		public string? Name { get; set; }
		public string? Fullname { get; set; }
		public string? Secret { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? Mail;
		public string? Role;
	}
}