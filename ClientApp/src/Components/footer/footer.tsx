import React, {useEffect, useState} from "react";
import {Article} from "../../models/models";
import {useNavigate, useParams} from "react-router-dom";
import {Box, Button, Card, CardContent, CardMedia, Grid, Typography} from "@mui/material";

export default function Footer() {
    const navigate = useNavigate();
    
    return (
        <Box>
            <Box sx={{ height: '5px', backgroundColor: '#9e1b32', width: '100%' }} />

            <Box
                className="footer-layout"
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'flex-start',
                    padding: '20px 40px',
                    gap: 1.5,
                }}
            >
                <Button
                    onClick={() => navigate(`/`)}
                    variant="contained"
                    sx={{
                        borderRadius: 0,
                        padding: "8px 24px",
                        fontWeight: 600,
                        backgroundColor: "#003366",
                        color: "#fff",
                        '&:hover': {
                            backgroundColor: "#115293",
                        },
                    }}
                >
                    ETN
                </Button>

                <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, mt: 2 }}>
                    <Button variant="text" sx={{ color: '#9e1b32', fontWeight: 600, padding: 0, minWidth: 0 }}>Home</Button>
                    <Button variant="text" sx={{ color: '#9e1b32', fontWeight: 600, padding: 0, minWidth: 0 }}>Latest</Button>
                    <Button variant="text" sx={{ color: '#9e1b32', fontWeight: 600, padding: 0, minWidth: 0 }}>For You</Button>
                </Box>

                <Typography variant="caption" color="text.secondary" sx={{ marginTop: 2 }}>
                    © {new Date().getFullYear()} ETN News. All rights reserved.
                </Typography>
            </Box>
        </Box>

    );
}