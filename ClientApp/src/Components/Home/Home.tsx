import { useEffect, useState } from "react";
import { Pagination, Stack } from "@mui/material";
import CarsCards from "./CarCards";
import NewsGrid from "./NewsGrid";
import {News} from "../../models/models";

interface Props {
    window?: () => Window;
}

const generateKeywords = (article: News): string[] => {
    const text = `${article.title} ${article.body ?? ''}`.toLowerCase();
    const words = text.match(/\b\w{5,}\b/g) || [];
    const unique = Array.from(new Set(words));
    return unique.slice(0, 5);
};

export default function Home(){
    const [article, setArticle] = useState<News[]>([]);
    const [page, setPage] = useState(1);

    const handleChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value);
    };

    useEffect(() => {
        const fetchNews = async () => {
            fetch('https://localhost:44300/api/News/GetAll', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ pagging: {page: page, count: 12}})
            }).then(res => res.json())
                .then(res => {
                    console.log(res.news.$values);
                    setArticle(res.news.$values);
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