import { useEffect, useState } from "react";
import { Pagination, Stack } from "@mui/material";
import CarsCards from "./CarCards";
import NewsGrid from "./NewsGrid";
import {Article, ArticleModel, NewsApiResponse} from "../../models/models";

interface Props {
    window?: () => Window;
}

const generateKeywords = (article: Article): string[] => {
    const text = `${article.title} ${article.description ?? ''}`.toLowerCase();
    const words = text.match(/\b\w{5,}\b/g) || [];
    const unique = Array.from(new Set(words));
    return unique.slice(0, 5);
};

export default function Home(){
    const [article, setArticle] = useState<Article[]>([]);

    useEffect(() => {
        const fetchNews = async () => {
            try {
                const res = await fetch(
                    'https://newsapi.org/v2/top-headlines?category=science&language=en&pageSize=10',
                    {
                        headers: {
                            Authorization: `Bearer cf199e18af1346f0b639c47d34607f31`,
                        },
                    }
                );
                const data: NewsApiResponse = await res.json();

                const enriched = data.articles.map(article => ({
                    ...article,
                    keywords: article.keywords ?? generateKeywords(article),
                }));

                setArticle(enriched);
            } catch (err) {
                console.error('Ошибка при загрузке новостей:', err);
            }
        };

        fetchNews();
    }, []);
    
    return(
        <div>
          <NewsGrid articles={article}/>
        </div>
    );
}