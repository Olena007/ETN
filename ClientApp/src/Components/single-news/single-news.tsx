import {Box, Card, CardContent, CardMedia, Grid, Typography} from "@mui/material";
import React, {useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import "./single-news.css"
import {styled} from "@mui/material/styles";
import {News} from "../../models/models";

function formatTime(iso: any): string {
    const date = new Date(iso);
    const now = new Date();
    const diff = Math.floor((now.getTime() - date.getTime()) / 1000 / 60 / 60);
    if (diff < 24) return `${diff} hrs ago`;
    return date.toLocaleDateString();
}

const Clamp = styled(Typography, {
    shouldForwardProp: (prop) => prop !== "lines",
})<{ lines: number }>(({ lines }) => ({
    display: "-webkit-box",
    WebkitLineClamp: lines,
    WebkitBoxOrient: "vertical",
    overflow: "hidden",
    textOverflow: "ellipsis",
}));

const defaultImage = "https://akhbarhub.ir/public/default-image/default-1080x1000.png";

export default function SingleNews() {
    const [article, setArticle] = useState<News>();
    const [related, setRelated] = useState<News[]>([]);
    const [more, setMore] = useState<News[]>([]);
    const {id} = useParams();
    const sideRelated = related?.slice(0, 4);
    const navigate = useNavigate();
    const moreNews = more?.filter(x => x.uri != id).slice(0, 10);

    const goToNews = (id: any) => {
        navigate(`/news/${id}`);
    };
    
    useEffect(() => {
        const fetchNews = async () => {
            const res = await fetch(`https://localhost:7001/api/Article/Get/uri?uri=${id}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                }
            });
            const data = await res.json();
            setArticle(data);
        };

        const relatedNews = async () => {
            const res = await fetch(`https://localhost:7001/api/Article/GetRecommended?uri=${id}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                }
            });
            const data = await res.json();
            setRelated(data);
        };

        const moreNews = async () => {
            const res = await fetch(`https://localhost:7001/api/Article/Get`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                }
            });
            const data = await res.json();
            
            setMore(data);
        };
        
        if (id) {
            fetchNews();
            relatedNews();
            moreNews();
        }
    }, [id]);


    return (
        <Box className="main-layout">
            <Card sx={{boxShadow: "none", borderRadius: 0}}>
                <CardContent>
                    <Box className="layout">
                        <Typography
                            variant="h4"
                            fontWeight={750}
                            sx={{marginBottom: "15px"}}>
                            {article?.title}
                        </Typography>
                        <Typography variant="caption" color="text.disabled">
                            {formatTime(article?.date.toString())}
                        </Typography>
                        <Typography sx={{marginTop: "15px"}} fontWeight={750}>
                            {article?.authors.map(x => x.name)}
                        </Typography>
                        <Typography color="text.disabled">
                            {article?.authors.map((x: { uri: any; }) => x.uri)}
                        </Typography>
                    </Box>
                    <Box className="image-layout">
                        <CardMedia
                            component="img"
                            height="500"
                            image={article?.image || defaultImage}
                            alt={article?.title ?? ""}
                        />
                    </Box>
                    <Box className="layout">
                        <Typography variant="body1" mt={1}>
                            {article?.body.split('\n').map((line: string | number | boolean | React.ReactElement<any, string | React.JSXElementConstructor<any>> | React.ReactFragment | React.ReactPortal | null | undefined, index: React.Key | null | undefined) => (
                                <span key={index}>
                                    {line}
                                    <br/>
                                </span>
                            ))}
                        </Typography>
                    </Box>
                </CardContent>
            </Card>
            <Box className="related-layout">
                <Box className="line"/>
                <Typography
                    variant="h5"
                    fontWeight={750}
                    color={'#9e1b32'}
                    sx={{marginBottom: "15px"}}>
                    Related
                </Typography>
                <Box className="related-news">
                    <Grid container spacing={2} direction="row" wrap="nowrap">
                        {sideRelated.map((article, index) => (
                            <Grid item key={index} xs={12} sm={6} md={3}>
                                <Box onClick={() => goToNews(article.uri)}>
                                    <Typography variant="subtitle1" fontWeight={600}>
                                        {article.title}
                                    </Typography>
                                    {article.body && (
                                        <Clamp
                                            variant="body2"
                                            color="text.secondary"
                                            lines={3}
                                            sx={{ mt: 0.5 }}
                                        >
                                            {article.body}
                                        </Clamp>
                                    )}
                                    <Typography variant="caption" color="text.disabled">
                                        {formatTime(article.date.toString())} &nbsp;|&nbsp;
                                    </Typography>
                                </Box>
                            </Grid>
                        ))}
                    </Grid>
                </Box>
            </Box>
            <Box className="related-layout">
                <Box className="line"/>
                <Typography
                    variant="h5"
                    fontWeight={750}
                    color={'#9e1b32'}
                    sx={{marginBottom: "15px"}}>
                    More
                </Typography>
                <Box className="related-news">
                    <Grid item xs={12} md={3}>
                        <Grid container direction="row" spacing={2}>
                            {moreNews.map((article, index) => (
                                <Grid item key={index}>
                                    <Box
                                        onClick={() => goToNews(article.uri)}
                                        className="related-news"
                                        sx={{ display: 'flex', gap: 2, cursor: 'pointer' }}
                                    >
                                        <Box sx={{ flex: 1, minWidth: '80px' }}>
                                            <Typography variant="caption" color="text.disabled">
                                                {formatTime(article.date.toString())}
                                            </Typography>
                                        </Box>

                                        <Box sx={{ flex: 6, display: 'flex', gap: 2, borderBottom: '1px solid #e0e0e0', pb: 1 }}>
                                            <Box sx={{ flex: 4 }}>
                                                <Typography variant="subtitle1" fontWeight={600}>
                                                    {article.title}
                                                </Typography>
                                                {article.body && (
                                                    <Clamp
                                                        variant="body2"
                                                        color="text.secondary"
                                                        lines={3}
                                                        sx={{ mt: 0.5 }}
                                                    >
                                                        {article.body}
                                                    </Clamp>
                                                )}
                                            </Box>

                                            <Box sx={{ flex: 2 }}>
                                                <img
                                                    src={article.image || defaultImage}
                                                    alt={article.title}
                                                    style={{ width: '100%', height: 'auto', borderRadius: 4 }}
                                                />
                                            </Box>
                                        </Box>
                                    </Box>
                                </Grid>
                            ))}
                        </Grid>
                    </Grid>
                </Box>
            </Box>
        </Box>
    );
}