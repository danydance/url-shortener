import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.scss'
import UrlInput from './components/URL_Input/url_input'
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

      {activeTab === "shortener" ? <UrlInput /> : <ALIAS />}
    </div>
  );
}

export default App
