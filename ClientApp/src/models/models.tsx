// Тип для Guid
type Guid = string;

// Главная новость
export interface News {
    newsId: Guid;
    uri: string;
    lang: string;
    date: string;
    time: string;
    dateTime: string;
    sim: number;
    url: string;
    title: string;
    body: string;
    links: string[];
    image?: string;
    eventUri?: string;
    views: View[];
    authors: Author[];
    categories: Category[];
    source: Source[];
    videos: Video[];
    location: Location[];
}

export interface Source {
    sourceId: Guid;
    newsId: Guid;
    uri: string;
    dataType?: string;
    title?: string;
    description?: string;
    location?: Location;
    locationValidated?: boolean;
    news: News;
}

export interface Location {
    locationId: Guid;
    newsId: Guid;
    type?: string;
    news: News;
    country: Country[];
}

export interface Country {
    countryId: Guid;
    locationId: Guid;
    type?: string;
    location: Location;
}

export interface Category {
    categoryId: Guid;
    newsId: Guid;
    uri: string;
    label?: string;
    wgt?: number;
    news: News;
}

export interface Author {
    authorId: Guid;
    newsId: Guid;
    uri: string;
    name?: string;
    type?: string;
    isAgency?: boolean;
    news: News;
}

export interface Video {
    videoId: Guid;
    newsId: Guid;
    uri: string;
    label?: string;
    news: News;
}

export interface View {
    // Структура View не указана в C# — добавь нужные поля сам
    viewId: Guid;
    // ...
}
