
class HomeController
{
	static index (req, res)
	{
		if(!req?.session?.isLogged)
		{
			res.redirect('/login');
			return;
		}

		if(req.session?.role === 'Admin')
		{
			res.redirect('/admin');
			return;
		}

		const data = {
			title: 'Home',
			name: req.session.name
		};

		res.render('home', data);
	}
}

module.exports = HomeController;
