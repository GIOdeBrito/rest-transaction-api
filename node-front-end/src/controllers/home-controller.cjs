
class HomeController
{
	static index (req, res)
	{
		// Redirects to the login page if is not logged in
		if(!req?.session?.isLogged)
		{
			res.redirect('/login');
			return;
		}

		// Redirects an admin user to the admin panel instead
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
