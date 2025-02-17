
class LoginModel
{
	static async requestLogin (name, secret)
	{
		const obj = {
			Name: name,
			Secret: secret
		};

		let response = await fetch('http://asp-app:8080/api/v1/login/login',
		{
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify(obj)
		});

		let json = await response.json();

		return json;
	}
}

module.exports = LoginModel;
