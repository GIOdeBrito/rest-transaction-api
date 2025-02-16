
class HomeController
{
	static index (req, res)
	{
		if(!req?.session?.isLogged)
		{
			res.redirect('/login');
		}

		res.render('home', { title: 'Home' });
	}
}

module.exports = HomeController;
