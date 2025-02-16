
class SigninController
{
	static index (req, res)
	{
		res.render('signin', { title: 'Signin' });
	}
}

module.exports = SigninController;
