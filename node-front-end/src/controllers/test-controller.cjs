
class TestController
{
	static async request (req, res)
	{
		let response = await fetch('http://asp-app:8080/api/v1/time');

		let json = await response.json();

		res.send({ result: JSON.stringify(json), message: "This is a response from the AspNetCore service." });
	}

	static showSession (req, res)
	{
		res.json(req.session);
	}
}

module.exports = TestController;
