export interface NewsModel {
    uri: string;
    lang: string;
    date: string;
    time: string;
    dateTime: string;
    sim: number;
    url: string;
    title: string;
    body: string;
    source: Source;
    authors: Author[];
    concepts: Concept[];
    categories: Category[];
    links: string[];
    videos: Video[];
    image: string;
    eventUri: string;
    location: Location;
    shares: Shares;
}

export interface Source {
    uri: string;
    dataType: string;
    title: string;
    description: string;
    location: Location;
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
    location: Location;
}

export interface Author {
    uri: string;
    name: string;
    type: string;
    isAgency: boolean;
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

export interface Shares {
    facebook: number;
}
