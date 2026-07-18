import React from 'react'
import './Homepage.css'
import { Button } from '@mui/material';
import { useState, useEffect } from 'react';
import axios from 'axios';
import apiClient from "../../api/apiClient";
import {session} from "../../models/Session";
import MessagePage from "../../components/MessagePage";
function Homepage() {
  const [sessions, setSessions] = useState([]);
  const [sessionId, setSessionId] = useState(null);
  const changeSession = (params) => {
    setSessionId(params);
  }
const deleteSession = (id) => {
  apiClient.delete('/Message/DeleteSession/' + id)
    .then(() => {
      const updatedSessions = sessions.filter(s => s.id !== id);
      setSessions(updatedSessions);
    })
    .catch(error => {
      console.error('Error:', error.response?.data?.message || error.message);
    });
};
  useEffect(() => {
    apiClient.post('/Message/GetSectionList')
      .then(response => {
        const data = response.data.map(
          s => session(s.id, s.title)
        );
        setSessions(data);
        setSessionId(data[0].id);
      })
      .catch(error => {
        console.error('Error:', error.response?.data?.message || error.message);
      });
  }, []);
  return (
    <div id='bodypage'>
        <div id="proplistbox">
        <div id='titlelist'>
        <Button
            variant="contained"
            onClick={() => changeSession(null)}
            sx={{
                backgroundColor: "rgb(30,30,30)",
                color: "white",
                "&:hover": {
                    backgroundColor: "rgb(50,50,50)",
                },
                "&:active": {
                    backgroundColor: "rgb(20,20,20)",
                },
                "&:focus": {
                    backgroundColor: "rgb(30,30,30)",
                }
            }}
        >
            New Chat
        </Button>
                {Array.isArray(sessions) ? sessions.map(session => (
                <div id='buttonstitle' key={session.id}>
                  <Button variant="text" id='titlebutton' onClick={()=>changeSession(session.id)}>{session.title}</Button>
                  <button id='deletebutton'onClick={()=>deleteSession(session.id)}>X</button>
                </div>
                )) : null}                    
            </div>
        </div>
        <MessagePage sessionId={sessionId} setSessions={setSessions} />
    </div>

  )
}

export default Homepage