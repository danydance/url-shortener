import { useState } from 'react'
import './url_input.scss'

function DisplayUrl({ url }) {
    if (!url) return null
    return <label>Shortening: {url}</label>
}

function UrlInput() {
    const [url, setUrl] = useState("") // starting with empty string
    const [submittedUrl, setSubmittedUrl] = useState("")

    const handleSubmit = (e) => {
        e.preventDefault()
        setSubmittedUrl(url)
    }
    return (
        <>
        <form onSubmit={handleSubmit}>
            <div className="segment">
            <h1>URL Sortener</h1>
            </div>

            <label>
            <input 
                type="text" 
                placeholder="https://Example/example/longurl" 
                value={url} 
                onChange={e => setUrl(e.target.value)}
                pattern='https?://.*' size="50"
                required 
            />
            </label>
            <button className="red" type="submit">
                Submit
            </button>
        </form>
        <DisplayUrl url={submittedUrl} />
        </>
  )
}

export default UrlInput
