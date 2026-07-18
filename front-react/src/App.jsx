import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import React, { useState, useEffect } from 'react';
import './App.css'
//import "bootstrap/dist/css/bootstrap.min.css";
import AutoAwesomeIcon from '@mui/icons-material/AutoAwesome';
import CopyrightIcon from '@mui/icons-material/Copyright';
import Button from '@mui/material/Button';
import { Route, Routes, useNavigate } from 'react-router';
import Homepage from './page/Homepage/Homepage';
import Contact from './page/Contact/Contact';
import About from './page/About/About';

function App() {
  const [count, setCount] = useState(0)
  const navigate = useNavigate();

  return (
    <>
    <header>
        <div id="headerbox">
          <div id="headerlogo">
            <Button variant="text" style={{ color: "white" }} onClick={()=>{navigate('/')}}><AutoAwesomeIcon fontSize='large' style={{ color: "white" }}></AutoAwesomeIcon></Button>
          </div>
          <div id="headerbuttons">
            <Button variant="text" style={{ color: "white" }} onClick={()=>{navigate('/About')}}>About</Button>
            <Button variant="text" style={{ color: "white" }} onClick={()=>{navigate('/Contact')}}>Contact</Button>
          </div>
        </div>
    </header>
    <main>
      <div id='mainbox'>
        <Routes>
          <Route path='/' element={<Homepage/>}/>
          <Route path='About' element={<About/>}/>
          <Route path='Contact' element={<Contact/>}/>
        </Routes>
      </div>

    </main>
    <footer>
      <div id='copyright'>
        <CopyrightIcon fontSize='small' style={{ color: "white" }} ></CopyrightIcon>
        <p>Buğra Daryal</p>
        <p>~</p>
        <p>03/07/2026</p>
      </div>
    </footer>

    </>
  )
}

export default App
