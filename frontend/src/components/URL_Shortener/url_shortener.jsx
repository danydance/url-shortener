import { useState } from 'react';
import send_to_api from '../../api/send_to_api'; 

function UrlShortener() {
    // State to store user input
    const [url, setUrl] = useState(''); // LongURL

    // State to store output or error
    const [shortUrl, setShortUrl] = useState(null); // ShortURL
    const [error, setError] = useState(null); // Error message
    const [isCopied, setIsCopied] = useState(false); // Copy Result

    // Submit handdler sends LongURL to API
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setShortUrl(null);
        setIsCopied(false);

        try {
            const result = await send_to_api(url); // Only sending URL
            setShortUrl(result);
        } catch (err) {
            setError(err.message);
        }
    };

    // Copies HortURL to clipboard
    const handleCopy = () => {
        if (!shortUrl) return;
        navigator.clipboard.writeText(shortUrl)
            .then(() => {
                setIsCopied(true);
                setTimeout(() => setIsCopied(false), 2000); // Waits 2 seconds to reset
            })
            .catch(() => console.error('Copy failed'));
    };

    return (
        <>
            <form onSubmit={handleSubmit}>
                <div className="segment">
                    <h1>URL Shortener</h1>
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
                </label>
                {/* Submit Button */}
                <div>
                    <button className="red" type="submit">Submit</button>
                </div>
                <br />
            </form>

            {/* Error message */}
            {error && <button>Error: {error}</button>}

            {/* Display ShortURL and copy option */}
            {shortUrl && (
                <button 
                    onClick={handleCopy}
                    style={{ cursor: shortUrl ? 'pointer' : 'not-allowed' }}
                >
                    {isCopied ? 'Copied to clipboard' : shortUrl}
                </button>
            )}
        </>
    );
}

export default UrlShortener;
