export type ArticleModel = {
    title: string;
    link: string;
    keywords: string[];
    creator: string[];
    videoUrl: string;
    description: string;
    content: string;
    pubDate: Date;
    fullDescription: string;
    imageUrl: string;
    sourceId: string;
};

export interface NewsApiResponse {
    status: string;
    totalResults: number;
    articles: Article[];
}

export interface Article {
    id: string;
    source: Source;
    author: string | null;
    title: string;
    description: string | null;
    url: string;
    urlToImage: string | null;
    publishedAt: string; 
    content: string | null;
    keywords?: string[] | null;
}

export interface Source {
    id: string | null;
    name: string;
}
