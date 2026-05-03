# Smart Movie Catalog — Product Context

## Purpose

**Smart Movie Catalog** is an AI-powered movie catalog designed as a portfolio-grade full-stack application.

The project demonstrates how a modern web application can combine a traditional movie catalog with practical AI features, including poster analysis, AI-generated metadata, smart discovery, and real-time user notifications.

The application is not intended to be a streaming platform. Its purpose is to show how movies can be registered, enriched, organized, searched, and presented through a clean product experience supported by modern backend, frontend, cloud, and AI engineering practices.

## Product Vision

Smart Movie Catalog helps users build and explore an intelligent movie catalog where each movie can be enhanced with both manually entered metadata and AI-generated insights.

The core experience is simple:

1. Register a movie.
2. Upload or associate a movie poster.
3. Analyze the poster with Gemini Vision.
4. Extract visual and textual metadata from the poster.
5. Search and browse movies using both catalog data and AI-generated tags.
6. Receive real-time notifications when new movies are added.

The project should feel like a real product, not a basic CRUD demo. It should communicate visual polish, technical maturity, incremental delivery, and practical AI integration.

## Problems It Solves

### 1. Movie catalogs are often limited to manual metadata

Traditional catalogs usually depend on fields such as title, year, genre, director, cast, and synopsis.

Smart Movie Catalog expands this by allowing AI to extract additional meaning from movie posters, such as:

- visible text;
- likely genres;
- visual tone;
- dominant colors;
- visible elements;
- possible themes;
- target audience signals;
- confidence score.

This makes the catalog richer without requiring every detail to be manually entered.

### 2. Poster images usually remain passive assets

In many applications, a poster is only displayed as an image.

In Smart Movie Catalog, the poster becomes a source of structured information. The system can analyze it and transform visual content into searchable metadata.

This creates a stronger connection between visual design, movie identity, and discovery.

### 3. Search is often too literal

Basic search usually finds movies only when the user types exact words that exist in the title or synopsis.

Smart Movie Catalog starts with simple smart search over both manual metadata and AI-generated tags. This allows users to discover movies by terms related to:

- themes;
- visual tone;
- genres;
- detected text;
- poster analysis;
- synopsis;
- cast and director.

Future versions may evolve this into semantic search, vector search, and RAG-based recommendations.

### 4. Portfolio projects often look like unfinished CRUD applications

Many portfolio projects demonstrate only basic forms, tables, and database operations.

Smart Movie Catalog is designed to go beyond that by combining:

- a polished frontend experience;
- a real backend API;
- PostgreSQL persistence;
- Docker deployment;
- cloud hosting;
- AI integration;
- real-time notifications;
- security and quality tooling;
- an incremental technical roadmap.

The project is meant to show product thinking and engineering maturity, not only coding ability.

### 5. AI features are often added without clear product value

This project uses AI for a concrete purpose: enriching movie data from posters.

The AI feature is not decorative. It directly supports catalog enrichment, visual discovery, filtering, search, and future recommendation flows.

## Target Users

### Primary user

A user who wants to register, browse, and organize movies in an intelligent catalog.

### Portfolio audience

Recruiters, technical interviewers, engineering managers, and architects evaluating the developer's ability to build a real-world application with:

- .NET;
- Angular;
- PostgreSQL;
- Docker;
- cloud deployment;
- AI integration;
- SignalR;
- clean architecture direction;
- CI/CD and quality practices.

### Future technical users

Developers or maintainers who want to evolve the application into a more advanced AI-powered catalog using:

- event-driven architecture;
- Inbox/Outbox;
- semantic search;
- Qdrant;
- embeddings;
- RAG;
- visual similarity search;
- observability.

## Core Value Proposition

Smart Movie Catalog turns a simple movie catalog into an intelligent discovery experience by combining structured metadata, poster analysis, smart search, and real-time interaction.

In one sentence:

> Smart Movie Catalog is an AI-powered movie catalog that lets users register movies, analyze posters with Gemini Vision, and discover films through smart search and real-time notifications.

## V1 Product Scope

The first complete version focuses on delivering a polished and demonstrable core product.

V1 includes:

- movie registration;
- movie listing;
- movie details;
- basic movie editing;
- poster upload or poster association;
- poster preview;
- Gemini Vision poster analysis;
- persisted AI analysis;
- AI-generated metadata display;
- basic smart search;
- sorting and browsing;
- real-time notification when a new movie is added;
- polished public home page;
- movie cards with flip interaction;
- loading, empty, success, and error states;
- basic responsive layout.

## V1 Non-Goals

The following are intentionally out of scope for V1:

- authentication;
- authorization;
- user profiles;
- favorites;
- streaming video;
- payments;
- Inbox/Outbox;
- event choreography;
- saga or process manager;
- Qdrant;
- semantic search;
- RAG;
- CLIP;
- visual embeddings;
- advanced observability;
- microservices;
- Kubernetes;
- production-grade HTTPS automation.

These items may be introduced in later milestones if they support the learning and portfolio goals.

## Future Evolution

The project is planned to evolve incrementally through clearly scoped milestones.

### V1 — Core Product

Deliver a complete catalog experience with movie registration, poster upload, Gemini Vision analysis, smart search, and SignalR notifications.

### V2 — Event-Driven Architecture

Introduce domain events, Outbox, Inbox, and event choreography where they provide architectural value.

### V3 — Semantic Search and RAG

Add embeddings, Qdrant, semantic search, and explainable recommendation responses.

### V4 — Vision and Poster Similarity

Add visual embeddings and poster similarity search.

### V5 — Advanced AI Architecture

Add incremental RAG, observability, workflow orchestration, and advanced operational maturity.

## Success Criteria

The project is successful when it can be demonstrated end-to-end as a real application:

1. The user opens the application.
2. The user registers a new movie.
3. The movie appears in the catalog.
4. Another browser tab receives a real-time notification.
5. The user opens the movie details page.
6. The user uploads or associates a poster.
7. The user analyzes the poster with AI.
8. The system displays structured AI-generated metadata.
9. The user searches using title, genre, synopsis, or AI-generated tags.
10. The movie appears in the results.
11. The application is deployed and accessible through a real cloud environment.

## Technical Positioning

Smart Movie Catalog should be implemented with a pragmatic architecture that supports incremental delivery.

The project should avoid unnecessary complexity in early versions. Architecture should evolve when the product needs justify it.

The V1 deployment model may use a single application container where the ASP.NET Core API serves the Angular frontend. This is acceptable because it reduces operational complexity and accelerates delivery.

Future versions can split frontend and backend containers if independent scaling, CDN hosting, or separate deployment pipelines become necessary.

## Engineering Principles

The project should follow these principles:

- deliver small, demonstrable increments;
- prefer working software over speculative architecture;
- keep the domain independent from infrastructure;
- isolate AI providers behind application interfaces;
- avoid exposing secrets in frontend code;
- document architectural decisions;
- use security and quality tools where they provide real value;
- keep the UI polished enough for portfolio demonstration;
- avoid presenting future features as already implemented;
- use the roadmap to show architectural evolution over time.

## Summary

Smart Movie Catalog exists to demonstrate how a simple catalog domain can become a rich, AI-enhanced product.

It solves the limitations of manual metadata, passive poster images, literal search, and low-quality portfolio demos by combining a polished product experience with practical AI, real-time notifications, cloud deployment, and a clear path toward advanced architecture.
