import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import { Link, Route, Routes } from 'react-router-dom';
import { Box, Container, Grid, Typography, Button } from '@mui/material';
import Home from './Components/Home/Home';
import Admin from './Components/Admin/Admin';
import Versions from './Components/Admin/Versions';
import Version from './Components/Admin/Version';
import Dashboard from './Components/Admin/Dashboard';
import Adding from './Components/Admin/Adding';
import Login from './Components/Login/Login';
import HomeHeader from "./Components/header/header";
import Header from './Components/header/header';

function App() {
  const [name, setName] = useState('');
  useEffect(() => {
    (
        async () => {
            const response = await fetch('https://localhost:7001/api/User/Client', {
                method: 'GET',
                headers: {'Content-Type': 'application/json'},
                credentials: 'include',
            });

            const content = await response.json();

            setName(content.email);
        }
    )();
});
  return (
    <div className="App">
        <Header>
            <Routes>
                <Route path="/" element={<Home/>}></Route>
                <Route path="admin" element={<Admin />}></Route>
                <Route path="login" element={<Login setName={setName}/>}></Route>
                <Route path="*" element={<NoMatchComponent />} />
            </Routes>
        </Header>
    </div>
  );
}

const NoMatchComponent = () => {
  return (
    <div className='errorPage'>
      <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '100vh'
      }}
    >
      <Container maxWidth="md">
        <Grid container spacing={2}>
          <Grid xs={6}>
            <Typography variant="h1">
              404
            </Typography>
            <Typography variant="h6">
              The page you’re looking for doesn’t exist.
            </Typography>
            <Button variant="contained"><Link to="/" style={{color: "white", textDecoration: "none"}}>Back Home</Link></Button>
          </Grid>
          <Grid xs={6}>
            <img
              src="https://cdn.pixabay.com/photo/2017/03/09/12/31/error-2129569__340.jpg"
              alt=""
              className='imgError'
              // width=30 height={250}
            />
          </Grid>
        </Grid>
      </Container>
    </Box>
    </div>
  );
}

export default App;
