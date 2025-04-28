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
import {Article, ArticleModel} from "../../models/models";
import {useNavigate} from "react-router-dom";

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

export default function NewsGrid(props: {articles: Article[]}) {
    const navigate = useNavigate();
    const sideLeft = props.articles.filter((_, i) => i === 1 || i === 2);
    const sideRight = props.articles.slice(4, 7);

    const thirdArticle = props.articles[3];

    const goToNews = (id: any) => {
        navigate(`/news/${id}`); 
    };

    return (
        <Box sx={{ p: 2, textAlign: "left", fontFamily: "Georgia, serif", marginLeft: "150px", marginTop: "50px", marginRight: "150px" }}>
            <Grid container spacing={2}>
                <Grid item xs={12} md={3}>
                    <Grid container direction="column" spacing={2}>
                        {sideLeft.map((article, index) => (
                            <Grid item key={index}>
                                <Card  onClick={() => goToNews(article.id)}
                                    sx={{
                                    display: "flex",
                                    flexDirection: "column",
                                    boxShadow: "none",
                                    borderRadius: 0 }} >
                                    <CardMedia
                                        component="img"
                                        height="140"
                                        image={article.urlToImage || defaultImage}
                                        alt={article.title}
                                    />
                                    <CardContent sx={{ p: 1.5 }}>
                                        <Clamp variant="subtitle1" lines={2} fontWeight={600}>
                                            {article.urlToImage ? (
                                                <Link href={article.urlToImage} target="_blank" underline="hover" color="inherit">
                                                    {article.title}
                                                </Link>
                                            ) : (
                                                article.title
                                            )}
                                        </Clamp>
                                        <Clamp variant="body2" color="text.secondary" lines={3}>
                                            {article.description}
                                        </Clamp>
                                        <Typography variant="caption" color="text.disabled" mt={1}>
                                            {formatTime(article.publishedAt.toString())} &nbsp;|&nbsp; {article.content}
                                        </Typography>
                                    </CardContent>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                </Grid>

                <Grid item xs={12} md={6}>
                    {thirdArticle && (
                        <Card sx={{
                            boxShadow: "none",
                            borderRadius: 0,
                        }}>
                            <CardMedia
                                component="img"
                                height="300"
                                image={thirdArticle.urlToImage || defaultImage}
                                alt={thirdArticle.title}
                            />
                            <CardContent>
                                <Typography variant="h5" fontWeight={700}>
                                    {thirdArticle.title}
                                </Typography>
                                <Typography variant="body1" color="text.secondary" mt={1}>
                                    {thirdArticle.description}
                                </Typography>
                                <Typography variant="caption" color="text.disabled" mt={2}>
                                    {formatTime(thirdArticle.publishedAt.toString())} &nbsp;|&nbsp; {thirdArticle.content}
                                </Typography>
                            </CardContent>
                        </Card>
                    )}
                </Grid>

                <Grid item xs={12} md={3}>
                    <Grid container direction="column" spacing={2}>
                        {sideRight.map((article, index) => (
                            <Grid item key={index}>
                                <Box>
                                    <Typography variant="subtitle1" fontWeight={600}>
                                        {article.title}
                                    </Typography>
                                    {article.description && (
                                        <Typography
                                            variant="body2"
                                            color="text.secondary"
                                            sx={{ mt: 0.5 }}
                                        >
                                            {article.description}
                                        </Typography>
                                    )}
                                    <Typography variant="caption" color="text.disabled">
                                        {formatTime(article.publishedAt.toString())} &nbsp;|&nbsp; {article.content}
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

