
class HomeController
{
	static index (req, res)
	{
		if(!req?.session?.isLogged)
		{
			res.redirect('/login');
		}

		const data = {
			title: 'Home',
			name: req.session.name
		};

		res.render('home', data);
	}
}

module.exports = HomeController;
