import React, {SyntheticEvent, useState} from "react";
import "react-bootstrap";
import {Navigate} from "react-router-dom";
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import {createTheme, ThemeProvider} from '@mui/material/styles';

import './register.css';

const theme = createTheme();

const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [redirect, setRedirect] = useState(false);

    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();

        fetch('https://localhost:7001/api/User/Register', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                email,
                password
            })
        });

        setRedirect(true);

    }

    if (redirect) {
        return <Navigate to="/login"/>;
    }


    return (
        //     <div className="form-register">

        //     <form onSubmit={submit} className="form-register-center">
        //         <h1 className="h3 mb-3 fw-normal">Please register</h1>
        //         <input type="email" className="form-control" placeholder="Email address" required  
        //         onChange={e => setEmail(e.target.value)}
        //         />

        //         <input type="password" className="form-control" placeholder="Password" required 
        //         onChange={e => setPassword(e.target.value)}
        //         />

        //         <Button className="w-100 btn btn-lg btn-primary" variant="success" type="submit">Submit</Button>

        //     </form>
        // </div>
        <ThemeProvider theme={theme}>
            <Container component="main" maxWidth="xs">
                <CssBaseline/>

                <Box
                    sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Avatar className="lock" sx={{m: 1, bgcolor: 'secondary.main'}}>
                        <LockOutlinedIcon/>
                    </Avatar>

                    <Typography component="h1" variant="h5">
                        Sign Up
                    </Typography>
                    <Box component="form" onSubmit={submit} noValidate sx={{mt: 1}}>
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            id="email"
                            label="Email Address"
                            name="email"
                            autoComplete="email"
                            autoFocus
                            onChange={e => setEmail(e.target.value)}
                        />
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Password"
                            type="password"
                            id="password"
                            autoComplete="current-password"
                            onChange={e => setPassword(e.target.value)}
                        />

                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{mt: 3, mb: 2}}
                            className="btn-signin"
                        >
                            Sign Up
                        </Button>
                    </Box>
                </Box>
            </Container>
        </ThemeProvider>
    );
}

export default Register;