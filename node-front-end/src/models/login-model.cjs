
const { httpPost } = require('../helpers/http.cjs');

class LoginModel
{
	static async requestLogin (name, secret)
	{
		const obj = {
			Name: name,
			Secret: secret
		};

		const url = 'http://asp-app:8080/api/v1/login/login';

		let response = await httpPost(url, obj);

		return response;
	}
}

module.exports = LoginModel;
