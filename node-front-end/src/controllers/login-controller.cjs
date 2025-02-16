
class LoginController
{
	static index (req, res)
	{
		res.render('login', { title: 'Login' });
	}
}

module.exports = LoginController;
