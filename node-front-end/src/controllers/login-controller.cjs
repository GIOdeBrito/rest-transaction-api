
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

			let result = await LoginModel.requestLogin(name, secret, req.session);

			if(!result)
			{
				res.redirect("/login");
				return;
			}

			console.log("session", req.session);

			res.redirect("/");
		}
		catch(ex)
		{
			console.error(ex);
			res.send("Error");
		}
	}
}

module.exports = LoginController;
