import { useState } from 'react'
import './App.scss'
import URLShortener from './components/URL_Shortener/url_shortener'
import ALIAS from './components/Alias/alias'

function App() {
  const [activeTab, setActiveTab] = useState("shortener"); // starting with shortener

  return (
    <div style={{ padding: "20px" }}>
      <div style={{
        display: 'flex',
        justifyContent: 'center',
        gap: '10px'
      }}>
        <h2
          onClick={() => setActiveTab("shortener")} 
          style={{
            padding: '10px 20px',
            fontWeight: activeTab === "shortener" ? "bold" : "normal",
            textDecoration: activeTab === "shortener" ? "underline" : "none"
          }}
        >
          URL Shortening
        </h2>

        <h2 
          onClick={() => setActiveTab("alias")} 
          style={{
            padding: '10px 20px',
            fontWeight: activeTab === "alias" ? "bold" : "normal",
            textDecoration: activeTab === "alias" ? "underline" : "none"
          }}
        >
          Alias
        </h2>
      </div>

      {activeTab === "shortener" ? <URLShortener /> : <ALIAS />}
    </div>
  );
}

export default App
