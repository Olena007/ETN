export interface Article {
    uri: string;
    lang: string;
    isDuplicate: boolean;
    date: string;
    time: string;
    dateTime: string;
    sim: number;
    url: string;
    title: string;
    body: string;
    source: Source;
    concepts: Concept[];
    categories: Category[];
    links: string[];
    videos: Video[];
    image: string;
    duplicateList: string[];
    originalArticle: Article | null;
    eventUri: string;
    location: Location | null;
    extractedDates: ExtractedDate[];
    shares: Shares;
    wgt: number;
    authors: Author[];
}

export interface ArticlesData {
    totalResults: number;
    page: number;
    count: number;
    pages: number;
    results: Article[];
}

export interface Source {
    uri: string;
    dataType: string;
    title: string;
    description: string;
    location: Location | null;
    locationValidated: boolean;
    ranking: Ranking;
}

export interface Location {
    type: string;
    label: Label;
}

export interface Label {
    eng: string;
}

export interface Ranking {
    importanceRank: number;
}

export interface Concept {
    uri: string;
    type: string;
    score: number;
    label: Label;
    image: string;
    trendingScore: TrendingScore;
    location: Location | null;
}

export interface TrendingScore {
    news: NewsScore;
}

export interface NewsScore {
    score: number;
    testPopFq: number;
    nullPopFq: number;
}

export interface Category {
    uri: string;
    label: string;
    wgt: number;
}

export interface Video {
    uri: string;
    label: string;
}

export interface ExtractedDate {
    amb: boolean;
    imp: boolean;
    date: string;
    textStart: number;
    textEnd: number;
}

export interface Shares {
    facebook: number;
}

export interface Author {
    uri: string;
    name: string;
    type: string;
    isAgency: boolean;
}
