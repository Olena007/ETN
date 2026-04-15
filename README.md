# Nachrichten-Empfehlungssystem

Ein personalisiertes Nachrichten-Empfehlungssystem basierend auf Vektor-Embeddings und multikriterieller Methodenauswahl. Dieses Projekt implementiert die Ergebnisse einer Forschungsstudie über Empfehlungsmethoden für Nachrichtenportale.

##  Über das Projekt

Diese Anwendung implementiert die Ergebnisse einer vergleichenden Studie über drei Arten von Text-Vektorisierern für die Personalisierung von Nachrichteninhalten:

- **OpenAI Embeddings** (`text-embedding-3-small`)
- **Sentence Transformers** (lokales Modell `all-MiniLM-L6-v2`)
- **Gemini Embeddings**

### Hauptfunktionen

- Semantische Analyse der Ähnlichkeit von Nachrichtenartikeln mittels Kosinus-Distanz
- Personalisierte Empfehlungen basierend auf dem Leseverlauf des Benutzers
- Vergleichende Bewertung von Embedding-Methoden mit Genauigkeitsmetriken (F1, Precision, Recall) und Antwortzeit
- Kaltstart-Behandlung für neue Artikel und neue Benutzer
- Modulare Architektur für einfachen Methodenwechsel

---------------------------------------------------------------------------------------------------------------------------------  

# News Recommendation System

A personalized news recommendation system based on vector embeddings and multi-criteria method selection. This project implements the findings of a research study on recommendation methods for news portals.

## About the Project

This application implements the results of a research study comparing three types of text vectorizers for news content personalization:

- **OpenAI Embeddings** (`text-embedding-3-small`)
- **Sentence Transformers** (local model `all-MiniLM-L6-v2`)
- **Gemini Embeddings**

### Key Features

- Semantic analysis of news article similarity using cosine distance
- Personalized recommendations based on user reading history
- Comparative evaluation of embedding methods using accuracy metrics (F1, Precision, Recall) and response time
- Cold-start handling for new articles and new users
- Modular architecture for easy method switching
