import {React,useEffect,useState} from 'react'
import {history} from "../models/History.js";
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import { Button } from '@mui/material';
import Face4Icon from '@mui/icons-material/Face4';
import SubdirectoryArrowRightIcon from '@mui/icons-material/SubdirectoryArrowRight';
import "./MessagePage.css";
import apiClient from "../api/apiClient.js";
import { useRef } from "react";
import { session } from '../models/Session.js';

function MessageItem({ sessionId, setSessions }) {
    const [title, setTitle] = useState('');
    const [histories, setHistories] = useState([]);
    const [getsessionId, setsessionId] = useState(null);
    const [getmessage, setmessage] = useState('');
    const messagesEndRef = useRef(null);

    const sendMessage = () => {
        console.log(getsessionId)
      apiClient.post('/Message/SendMessage',{
        message: getmessage,
        sessionId: getsessionId ?? null
      })
      .then(response => {
        if (getsessionId == null && response.data.title != null) {

            setTitle(response.data.title);

            setsessionId(response.data.session_id);

            setSessions(prev => [
                {
                    id: response.data.session_id,
                    title: response.data.title
                },
                ...(Array.isArray(prev) ? prev : [])
            ]);
        }
        const userMessage = {
            id: response.data.user_id,
            message: getmessage,
            role: 0,
            date: new Date(Date.now())
        }
        const aiMessage = {
            id: response.data.ai_id,
            message: response.data.ai_message,
            role: 1,
            date: new Date(response.data.ai_message_date)
        }
        setmessage("");
        setHistories(prev => [
            ...(Array.isArray(prev) ? prev : []),
            userMessage,
            aiMessage
        ])
      })
      .catch(error => {
        console.error('Error:', error.response?.data?.message || error.message);
      });
    };
    useEffect(() => {
        setsessionId(sessionId);
    }, [sessionId]);
    useEffect(() => {
          if(sessionId !== null) {
            apiClient.post('/Message/MessageHistory',  sessionId)
            .then(response => {
                setTitle(response.data.title);
                const data = response.data.history.map(
                    h => history(h.id,h.message, h.role, h.date)
                );
                setHistories(data);
                })
                .catch(error => {
                    console.error('Error:', error.response?.data?.message || error.message);
                });
          }
          else{
            setmessage('');
            setHistories(null);
            setTitle('');
          }
    }, [sessionId]);
    useEffect(() => {
        messagesEndRef.current?.scrollIntoView({
            behavior: "smooth"
        });
    }, [histories]);
  return (
        <div id='listbox'>
        <div id="titlediv"
            style={{
                display: title ? "block" : "none",
                borderBottom: !title ?? "none"
            }}>
            <b>{title}</b>
        </div>
            <div id='messagelistbox'>
            {
                Array.isArray(histories) 
                ? histories.map((history) => {
                    return (
                        <div id="messagebox" key={`${history.role}-${history.id}`}>
                            <div id="messageboxicon">
                                {
                                    history.role === 0
                                    ? <Face4Icon fontSize="large" />
                                    : <SubdirectoryArrowRightIcon fontSize="large" />
                                }
                            </div>

                            <div id="messagesector">
                                <p>{history.message}</p>
                                <p>{history.date.toLocaleString("tr-TR", {
                                    day: "2-digit",
                                    month: "2-digit",
                                    year: "numeric",
                                    hour: "2-digit",
                                    minute: "2-digit"
                                })}</p>
                            </div>
                        </div>
                    );
                })
                : null
            }
            <div ref={messagesEndRef} />
            </div>
            <div id='messagesendbox'>
                <div style={{width:'95%'}}>
                    <Box
                    component="form"
                    sx={{ '& .MuiTextField-root': {width: '100%'} }}
                    noValidate
                    autoComplete="off"
                    >
                    <div id='multiboxsector'>
                        <TextField
                        required
                        id="outlined-multiline-flexible"
                        label="Please Text Me"
                        multiline
                        maxRows={2}
                        value={getmessage}
                        onChange={(e) => setmessage(e.target.value)}
                        sx={{
                            "& .MuiOutlinedInput-root": {
                            "& textarea": {
                                overflow: "hidden",
                            },

                            "& fieldset": {
                                borderColor: "black",
                            },

                            "&:hover fieldset": {
                                borderColor: "black",
                            },

                            "&.Mui-focused fieldset": {
                                borderColor: "black",
                                borderWidth: "2px",
                            },
                            },

                            "& .MuiInputLabel-root": {
                            color: "black",
                            },

                            "& .MuiInputLabel-root.Mui-focused": {
                            color: "black",
                            },
                        }}
                        />
                    </div>
                    </Box>
                </div>
                    <Button
                    variant="contained"
                    sx={{
                        backgroundColor: "black",
                        color: "white",
                        "&:hover": {
                        backgroundColor: "#333",
                        },
                    }}
                    onClick={()=>sendMessage()}
                    >
                    Send
                    </Button>
            </div>
        </div>
  );
}

export default MessageItem;