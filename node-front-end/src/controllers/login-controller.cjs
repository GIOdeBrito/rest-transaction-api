
const LoginModel = require('../models/login-model.cjs');

class LoginController
{
	static index (req, res)
	{
		res.render('login', { title: 'Login' });
	}

	static async login (req, res)
	{
		try
		{
			let name = req.body.name;
			let secret = req.body.secret;

			let json = await LoginModel.requestLogin(name, secret);

			if(json?.error === true)
			{
				res.redirect("/login");
				return;
			}

			res.json(json);
		}
		catch(ex)
		{
			console.error(ex);
			res.send("Error");
		}
	}
}

module.exports = LoginController;
