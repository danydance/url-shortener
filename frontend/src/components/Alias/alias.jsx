import { useState } from 'react'
import send_alias_to_api from '../../api/send_alias_to_api'

function Alias() {
    const [url, setUrl] = useState("")
    const [alias, setAlias] = useState("")
    const [shortUrl, setShortUrl] = useState("")
    const [error, setError] = useState("")
    const [isCopied, setIsCopied] = useState(false)

    const handleSubmit = async (e) => {
        e.preventDefault()
        setError("")
        setShortUrl("")

        try {
            const result = await send_alias_to_api(url, alias)
            setShortUrl(result)
        } catch (err) {
            setError(err.message)
        }
    }

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
                
                <div>
                    <button className="red" type="submit">Submit</button>
                </div>
                <br />
            </form>
            
            {error && <button style={{ color: 'red' }}>{error}</button>}

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
