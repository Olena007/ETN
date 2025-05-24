import React from "react";
import {
    Box,
    Card,
    CardContent,
    CardMedia,
    Grid,
    Typography,
    Link,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import {useNavigate} from "react-router-dom";
import {News} from "../../models/models";

const defaultImage = "https://akhbarhub.ir/public/default-image/default-1080x1000.png";

function formatTime(iso: string): string {
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

export default function NewsGrid(props: {articles: News[]}) {
    const navigate = useNavigate();
    
    const sideLeft = props.articles?.filter((_, i) => i === 1 || i === 2);
    const sideRight = props.articles?.slice(4, 7);

    const thirdArticle = props.articles[3];

    const goToNews = (id: any) => {
        navigate(`/news/${id}`); 
    };

    return (
        <Box
            sx={{
                p: 2,
                px: 4,   
                fontFamily: "Georgia, serif",
                mx: "auto",
                mt: 6,
                maxWidth: "1200px",
                textAlign: "left",
            }}>
            <Grid container spacing={3}>
                {/* Левая колонка */}
                <Grid item xs={12} md={3}>
                    <Grid container direction="column" spacing={3} >
                        {sideLeft.map((article, index) => (
                            <Grid item key={index} wrap="wrap">
                                <Box onClick={() => goToNews(article.uri)} sx={{ cursor: "pointer" }}>
                                    <CardMedia
                                        component="img"
                                        height="150"
                                        image={article.image || defaultImage}
                                        alt={article.title}
                                    />
                                    <Typography variant="subtitle1" fontWeight={600}>
                                        {article.title}
                                    </Typography>
                                    {article.body && (
                                        <Clamp
                                            variant="body2"
                                            color="text.secondary"
                                            lines={3}
                                            sx={{ mt: 0.5, wordBreak: 'break-word' }}
                                        >
                                            {article.body}
                                        </Clamp>
                                    )}
                                    <Typography variant="caption" color="text.disabled">
                                        {formatTime(article.date.toString())}
                                    </Typography>
                                </Box>
                            </Grid>
                        ))}
                    </Grid>
                </Grid>

                {/* Центральная большая новость */}
                <Grid item xs={12} md={5}>
                    {thirdArticle && (
                        <Card
                            onClick={() => goToNews(thirdArticle.uri)}
                            sx={{
                                boxShadow: "none",
                                borderRadius: 0,
                                cursor: "pointer",
                            }}
                        >
                            <CardMedia
                                component="img"
                                height="300"
                                image={thirdArticle.image || defaultImage}
                                alt={thirdArticle.title}
                            />
                            <CardContent>
                                <Clamp variant="h5" lines={2} fontWeight={700}>
                                    {thirdArticle.title}
                                </Clamp>
                                <Clamp variant="body1" color="text.secondary" lines={10} mt={1}>
                                    {thirdArticle.body}
                                </Clamp>
                                <Typography variant="caption" color="text.disabled" mt={2}>
                                    {formatTime(thirdArticle.date.toString())}
                                </Typography>
                            </CardContent>
                        </Card>
                    )}
                </Grid>

                {/* Правая колонка */}
                <Grid item xs={12} md={3}>
                    <Grid container direction="column" spacing={3}>
                        {sideRight.map((article, index) => (
                            <Grid item key={index}>
                                <Box onClick={() => goToNews(article.uri)} sx={{ cursor: "pointer" }}>
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
                                        {formatTime(article.date.toString())}
                                    </Typography>
                                </Box>
                            </Grid>
                        ))}
                    </Grid>
                </Grid>
            </Grid>
        </Box>
    );
}

