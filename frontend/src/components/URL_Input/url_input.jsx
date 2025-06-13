import { useState } from 'react'
import './url_input.scss'

function UrlInput() {
  return (
    <>
      <form>
        <div className="segment">
          <h1>Url Shortener</h1>
        </div>

        <label>
          <input type="text" placeholder="https://Example/example/longurl" />
        </label>
        <button className="red" type="button">
          <i className="icon ion-md-lock"></i> Submit
        </button>
      </form>
    </>
  )
}

export default UrlInput
