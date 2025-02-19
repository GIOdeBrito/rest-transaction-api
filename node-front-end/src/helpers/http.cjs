
/**
 * Function that sends a JSON post request to an endpoint.
 * @param {string} url
 * @param {object} data
 * @returns {Promise<object | string>}
 */
async function httpPost (url, data)
{
	let response = await fetch(url,
	{
		method: "POST",
		headers: {
			"Content-Type": "application/json"
		},
		body: JSON.stringify(data)
	});

	let responseClone = response.clone();

	try
	{
		return await response.json();
	}
	catch(ex)
	{
		console.error(ex);
		return await response.text();
	}
}

module.exports = {
	httpPost
};
