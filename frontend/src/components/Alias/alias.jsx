import { useState } from 'react'
import send_to_api from '../../api/send_to_api'

function Alias() {
    // State to store user input
    const [url, setUrl] = useState("") // LongURL
    const [alias, setAlias] = useState("") // ALIAS

    // State to store output or error
    const [shortUrl, setShortUrl] = useState("") // ShortURL
    const [error, setError] = useState("") // Error message
    const [isCopied, setIsCopied] = useState(false) // Copy Result

    // Submit handler sends URL + ALIAS to API
    const handleSubmit = async (e) => {
        e.preventDefault()
        setError("")
        setShortUrl("")

        try {
            const result = await send_to_api(url, alias)
            setShortUrl(result)
        } catch (err) {
            setError(err.message)
        }
    }

    // Copies ShortURL to clipboard
    const handleCopy = () => {
        if (!shortUrl) return
        navigator.clipboard.writeText(shortUrl).then(() => {
            setIsCopied(true)
            setTimeout(() => setIsCopied(false), 2000)
        })
    }

    return (
        <>
            <form onSubmit={handleSubmit}>
                <div className="segment">
                    <h1>Alias</h1>
                </div>
                {/* URL and ALIAS input fields */}
                <label>
                    <input 
                        type="text" 
                        placeholder="Enter your URL" 
                        value={url} 
                        onChange={e => setUrl(e.target.value)}
                        pattern='https?://.*' 
                        size="50"
                        required 
                    />
                    <br /><br />
                    <input 
                        type="text" 
                        placeholder="Enter your custom alias" 
                        value={alias} 
                        onChange={e => setAlias(e.target.value)}
                        pattern='[a-zA-Z0-9\-]*' 
                        size="50"
                        required 
                    />
                </label>

                {/* Submit Button */}
                <div>
                    <button className="red" type="submit">Submit</button>
                </div>
                <br />
            </form>

            {/* Error message */}
            {error && <button style={{ color: 'red' }}>{error}</button>}

            {/* Display ShortURL and copy option */}
            {shortUrl && (
                <button 
                    onClick={handleCopy}
                    style={{ cursor: "pointer" }}
                >
                    {isCopied ? "Copied!" : shortUrl}
                </button>
            )}
            
        </>
    )
}

export default Alias
