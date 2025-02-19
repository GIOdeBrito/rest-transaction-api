
const SigninModel = require('../models/signin-model.cjs');

class SigninController
{
	static index (req, res)
	{
		res.render('signin', { title: 'Signin' });
	}

	static async createNewUser (req, res)
	{
		try
		{
			let firstname = req.body.name;
			let surname = req.body.surname;
			let secret = req.body.secret;
			let mail = req.body.mail;

			let response = await SigninModel.registerNewUser(firstname, surname, secret, mail);

			if('error' in response)
			{
				res.redirect('/signin');
				return;
			}

			res.redirect('/login');
		}
		catch(ex)
		{
			console.error(ex);
			res.send("Error");
		}
	}
}

module.exports = SigninController;
