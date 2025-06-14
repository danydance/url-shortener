import { useEffect, useState } from "react";
import send_to_api from '../../api/send_to_api';

function ApiHandler({ url }) {
  const [shortUrl, setShortUrl] = useState(null);
  const [error, setError] = useState(null);
  const [isCopied, setIsCopied] = useState(false);

  useEffect(() => {
    /*
    this useEffect is used for multible stuff
    1. to collect the data(shorturl/redirect) from the send_to_api.js
    2. the reactive button when you submit a url
    */
    if (!url) return;

    setShortUrl(null); // reset on new url
    setIsCopied(false);
    send_to_api(url)
      .then(data => {
        setShortUrl(data); // Sets the shorturl to what send_to_api returns
      })
      .catch(err => setError(err.message));
  }, [url]);
  
  const handleCopy = () => {
    if (!shortUrl) return;
    navigator.clipboard.writeText(shortUrl)
      .then(() => {
        setIsCopied(true);
        // resets to show url on button after 2 seconds
        setTimeout(() => setIsCopied(false), 2000);
      })
      .catch(() => {
        console.log("Error Copy URL")
      });
  };

  if (error) {
    return <label>Error: {error}</label>;
  }

  return (
    <button 
      onClick={handleCopy} 
      disabled={!shortUrl} // disabled while loading or no url
      style={{ cursor: !shortUrl ? "not-allowed" : "pointer" }} // changes the cursor when you hover on the loading button
    >
      { !shortUrl ? "Loading..." : isCopied ? "Copied to clipboard" : shortUrl }
    </button>
  );
}

export default ApiHandler;
