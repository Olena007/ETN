import { useEffect, useState } from "react";
import { Pagination, Stack } from "@mui/material";
import CarsCards from "./CarCards";
import NewsGrid from "./NewsGrid";
import {Article} from "../../models/models";

interface Props {
    window?: () => Window;
}

const generateKeywords = (article: Article): string[] => {
    const text = `${article.title} ${article.body ?? ''}`.toLowerCase();
    const words = text.match(/\b\w{5,}\b/g) || [];
    const unique = Array.from(new Set(words));
    return unique.slice(0, 5);
};

export default function Home(){
    const [article, setArticle] = useState<Article[]>([]);

    useEffect(() => {

        const fetchNews = async () => {
            fetch('https://localhost:7001/api/Article/Get', {
                method: 'GET',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                }
            }).then(res => res.json())
                .then(res => {
                    setArticle(res);
                });
        }
        
        fetchNews();
    }, []);
    
    return(
        <div>
          <NewsGrid articles={article}/>
        </div>
    );
}