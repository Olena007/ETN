import {Box, Card, CardContent, CardMedia, Grid, Typography} from "@mui/material";
import React, {useEffect, useState} from "react";
import {Article} from "../../models/models";
import {useParams} from "react-router-dom";

function formatTime(iso: string): string {
    const date = new Date(iso);
    const now = new Date();
    const diff = Math.floor((now.getTime() - date.getTime()) / 1000 / 60 / 60);
    if (diff < 24) return `${diff} hrs ago`;
    return date.toLocaleDateString();
}

const defaultImage = "https://akhbarhub.ir/public/default-image/default-1080x1000.png";

export default function SingleNews() {
    const [article, setArticle] = useState<Article | null>(null);
    const {id} = useParams();

    useEffect(() => {
        const fetchNews = async () => {
            const res = await fetch(`https://localhost:7001/api/Article/Get?id=${id}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                }
            });
            const data = await res.json();
            setArticle(data);
        };

        if (id) {
            fetchNews();
        }
    }, [id]);


    return (
        <Box sx={{
            p: 2,
            textAlign: "left",
            fontFamily: "Georgia, serif",
            marginLeft: "150px",
            marginTop: "50px",
            marginRight: "150px"
        }}>
            <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                    <Card sx={{boxShadow: "none", borderRadius: 0}}>
                        <CardMedia
                            component="img"
                            height="300"
                            image={article?.urlToImage || defaultImage}
                            alt={article?.title ?? ''}
                        />
                        <CardContent>
                            <Typography variant="h5" fontWeight={700}>
                                {article?.title}
                            </Typography>
                            <Typography variant="body1" color="text.secondary" mt={1}>
                                {article?.description}
                            </Typography>
                            <Typography variant="caption" color="text.disabled" mt={2}>
                                {article?.publishedAt ? formatTime(article.publishedAt) : ''} &nbsp;|&nbsp; {article?.content}
                            </Typography>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
        </Box>
    );
}