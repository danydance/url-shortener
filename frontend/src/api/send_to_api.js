const URL = 'http://localhost:5001/api/shorten'

async function send_to_api(url) {
    const response = await fetch(URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ url })
    })

    if (!response.ok) {
        throw new Error('Error shortening URL') 
    }
    const data = await response.json()
    console.log(data)
    return data
}

export default send_to_api
