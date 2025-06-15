const URL = 'http://localhost:5001/api/alias';

async function send_alias_to_api(url, alias) {
    const response = await fetch(URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ url, alias })  // sending both of the requests
    });

    if (!response.ok) {
        const errorData = await response.json(); // errorData = the error that the server sends
        throw new Error(errorData);
    }

    const data = await response.json();
    console.log(data);
    return data;
}

export default send_alias_to_api;
