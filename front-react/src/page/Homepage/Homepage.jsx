import React from 'react'
import './Homepage.css'
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import { Button } from '@mui/material';
import Face4Icon from '@mui/icons-material/Face4';
import SubdirectoryArrowRightIcon from '@mui/icons-material/SubdirectoryArrowRight';
import { useState, useEffect } from 'react';
import axios from 'axios';

function Homepage() {
  const [message, setMessage] = useState('');
    useEffect(() => {

    })
  return (
    <div id='bodypage'>
        <div id='messagelistbox'>
            <div id='messagebox'>
                <div id='messageboxicon'>
                    <Face4Icon fontSize="large" />
                </div>
                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean malesuada quam tellus, ut porta risus tincidunt eget. Aenean rhoncus placerat mi a tristique. Aliquam eu lorem ullamcorper, porta nulla suscipit, imperdiet risus. Etiam interdum consectetur tellus in facilisis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Curabitur eleifend, dolor id vestibulum vestibulum, risus mauris sodales enim, quis luctus magna est sed ipsum. Donec vestibulum risus sit amet neque pellentesque feugiat. Quisque tincidunt vel magna in rutrum. Nam ullamcorper massa at imperdiet interdum. Donec neque metus, ornare non erat non, porttitor tempus justo. Integer vehicula est vitae leo consectetur tincidunt. Quisque sit amet purus nec odio sagittis dapibus eu sed ex. Aenean tincidunt pulvinar nibh.</p>
            </div>
            <div id='messagebox'>
                <div id='messageboxicon'>
                    <SubdirectoryArrowRightIcon fontSize="large" />
                </div>
                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean malesuada quam tellus, ut porta risus tincidunt eget. Aenean rhoncus placerat mi a tristique. Aliquam eu lorem ullamcorper, porta nulla suscipit, imperdiet risus. Etiam interdum consectetur tellus in facilisis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Curabitur eleifend, dolor id vestibulum vestibulum, risus mauris sodales enim, quis luctus magna est sed ipsum. Donec vestibulum risus sit amet neque pellentesque feugiat. Quisque tincidunt vel magna in rutrum. Nam ullamcorper massa at imperdiet interdum. Donec neque metus, ornare non erat non, porttitor tempus justo. Integer vehicula est vitae leo consectetur tincidunt. Quisque sit amet purus nec odio sagittis dapibus eu sed ex. Aenean tincidunt pulvinar nibh.</p>
            </div>
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
                >
                Send
                </Button>
        </div>
    </div>

  )
}

export default Homepage