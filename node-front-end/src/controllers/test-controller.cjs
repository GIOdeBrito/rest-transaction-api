
//const http = require('node:http');

class TestController
{
	static async request (req, res)
	{
		let response = await fetch('http://asp-app:8080/api/v1/time');

		let json = await response.json();

		res.send({ result: JSON.stringify(json), message: "This is a response from the AspNetCore service." });
	}
}

module.exports = TestController;
