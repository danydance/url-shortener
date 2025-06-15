import { useState } from 'react';
import './url_input.scss';
import send_to_api from '../../api/send_to_api'; 

function UrlInput() {
    const [url, setUrl] = useState('');
    const [shortUrl, setShortUrl] = useState(null);
    const [error, setError] = useState(null);
    const [isCopied, setIsCopied] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setShortUrl(null);
        setIsCopied(false);

        try {
            const result = await send_to_api(url);
            setShortUrl(result);
        } catch (err) {
            setError(err.message);
        }
    };

    const handleCopy = () => {
        if (!shortUrl) return;
        navigator.clipboard.writeText(shortUrl)
            .then(() => {
                setIsCopied(true);
                setTimeout(() => setIsCopied(false), 2000);
            })
            .catch(() => console.error('Copy failed'));
    };

    return (
        <>
            <form onSubmit={handleSubmit}>
                <div className="segment">
                    <h1>URL Shortener</h1>
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
                </label>
                <div>
                    <button className="red" type="submit">Submit</button>
                </div>
                <br />
            </form>

            {error && <button>Error: {error}</button>}
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

export default UrlInput;
