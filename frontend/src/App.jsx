import { useState } from 'react'
import './App.scss'
import URLShortener from './components/URL_Shortener/url_shortener'
import ALIAS from './components/Alias/alias'
import { AnimatePresence, motion } from "framer-motion";



function App() {
  const [activeTab, setActiveTab] = useState("shortener"); // starting with shortener
  const [prevTab, setPrevTab] = useState("shortener"); // saves the previous tab

  const handleTabChange = (tab) => { // when change it will switch directions
    setPrevTab(activeTab);
    setActiveTab(tab);
  };

  // decide slide direction(if activate tab is either alias or shortener then diraction 1 else -1)
  const direction = activeTab === "alias" && prevTab === "shortener" ? 1 : -1; 

  return (
    <div style={{ padding: "20px" }}>
      <div style={{
        display: 'flex',
        justifyContent: 'center',
        gap: '10px'
      }}>
        <h2
          onClick={() => handleTabChange("shortener")} // Selected shortener
          style={{
            padding: '10px 20px',
            fontWeight: activeTab === "shortener" ? "bold" : "normal",
            textDecoration: activeTab === "shortener" ? "underline" : "none",
            cursor : 'pointer',
          }}
        >
          URL Shortening
        </h2>

        <h2 
          onClick={() => handleTabChange("alias")} // Selected alias
          style={{
            padding: '10px 20px',
            fontWeight: activeTab === "alias" ? "bold" : "normal",
            textDecoration: activeTab === "alias" ? "underline" : "none",
            cursor : 'pointer',
          }}
        >
          Alias
        </h2>
      </div>
      {/* Slide Animation */}
      <AnimatePresence mode="wait">
        <motion.div
          key={activeTab}
          initial={{ x: -300 * direction, opacity: 0 }}
          animate={{ x: 0, opacity: 1 }}
          exit={{ x: 300 * direction, opacity: 0 }}
          transition={{ duration: 0.4, ease: "easeInOut" }}
          >
          {/* If the activeTab that was selected was shortener show UrlShortener form else show Alias form */}
          {activeTab === "shortener" ? <URLShortener /> : <ALIAS />} 
        </motion.div>
      </AnimatePresence>
    </div>
  );
}

export default App
