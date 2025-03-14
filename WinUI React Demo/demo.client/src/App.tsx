import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { HubConnectionBuilder } from '@microsoft/signalr'

function App() {
  const [count, setCount] = useState(0)
  const [message, setMessage] = useState("")
  const [pingMessage, setPingMessage] = useState("")

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/ping', { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(() => {
        console.log("Connection started!");
      }).catch((error) => {
        console.error("Error: " + error);
      });

    connection.on("Ping", (response) => {
      setPingMessage(response.message);
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
      <div>Ping: {pingMessage}</div>
      <button onClick={() => handleClickMe()}>Click me!</button>
      {message && <div>{message}</div>}
    </>
  )

  async function handleClickMe() {
    //fetch("http://localhost:5000/").then((res) => res.text()).then(setMessage)
    try {
      const res = await fetch("http://localhost:5000/")
      const text = await res.json()
      setMessage(text.message)
    }
    catch (error) {
      alert(error)
    }
  }
}

export default App
