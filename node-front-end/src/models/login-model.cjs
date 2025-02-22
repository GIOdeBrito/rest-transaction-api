
const { httpPost } = require('../helpers/http.cjs');

class LoginModel
{
	/**
	 * Checks wether the username and password are a match
	 * for an user in the database.
	 * @param {string} name
	 * @param {string} secret
	 * @returns {boolean}
	 */
	static async requestLogin (name, secret, session)
	{
		const obj = {
			Name: name,
			Secret: secret
		};

		const url = 'http://asp-app:8080/api/v1/login/login';

		let response = await httpPost(url, obj);

		if(response?.error === true)
		{
			return false;
		}

		this.#setSessionVars(session, response);

		return true;
	}

	static #setSessionVars (session, response)
	{
		session.isLogged = true;
		session.name = response.name;
		session.fullname = response.fullname;
		session.createdat = response.createdat;
		session.role = response.role;
	}
}

module.exports = LoginModel;
