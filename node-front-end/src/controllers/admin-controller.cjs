
class AdminController
{
	static index (req, res)
	{
		if(!req?.session?.isLogged)
		{
			res.redirect('/login');
			return;
		}

		const data = {
			title: 'Panel'
		};

		res.render('admin', data);
	}
}

module.exports = AdminController;
