
const { httpPost } = require('../helpers/http.cjs');

class SigninModel
{
	static async registerNewUser (name, surname, secret, mail)
	{
		const obj = {
			Name: name,
			Surname: surname,
			Secret: secret,
			Mail: mail
		};

		console.log(obj);

		const url = 'http://asp-app:8080/api/v1/auth/registernew';

		let response = await httpPost(url, obj);

		return response;
	}
}

module.exports = SigninModel;
