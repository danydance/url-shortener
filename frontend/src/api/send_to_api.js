const SHORTEN_URL = 'http://localhost:5001/api/shorten';
const ALIAS_URL = 'http://localhost:5001/api/alias';

// Sends a POST request to the API to shorten a LongURL
async function send_to_api(url, alias = null) {
    const endpoint = alias ? ALIAS_URL : SHORTEN_URL; // Desides which API endpoint based on alias

    const body = alias ? {url, alias} : {url}; // Body based on just URL or URL + ALIAS

    const response = await fetch(endpoint, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json' // Sending JSON data
        },
        body: JSON.stringify(body) // This request sends the LongUrl to shorten it
    })

    // If the response is not OK it will give an Error message.
    if (!response.ok) {
        const errorData = await response.json(); // errorData = the error that the server sends
        throw new Error(errorData); // throws error message
    }
    const data = await response.json() // Recieving response from server
    console.log(data)
    return data // Returns the ShortenURL
}

export default send_to_api
