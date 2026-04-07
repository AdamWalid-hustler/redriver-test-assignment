# RedRiverTest — Book & Citation Manager

A full-stack web application for managing books and personal citations, built with **.NET 9 Web API** and **Angular 20**. Users can register, log in, and perform full CRUD operations on books and their own citation collection.

Built as part of the **Red River internship acceptance project**.

## Features

- **JWT Authentication** — Register and log in; tokens are issued by the API and attached to requests via an Angular HTTP interceptor.
- **Book Management** — View, add, edit, and delete books (shared across all users).
- **My Citations** — Each user gets a personal collection of quotes with full CRUD. Seeded with 5 default quotes on first visit.
- **Dark / Light Theme** — Toggle with automatic OS preference detection, persisted in `localStorage`.
- **Responsive UI** — Bootstrap 5 grid and components with Font Awesome icons.
- **API Documentation** — Scalar API reference available at `/scalar` in development.

## Tech Stack

| Layer     | Technology                                          |
| --------- | --------------------------------------------------- |
| Backend   | .NET 9, ASP.NET Core Web API, Entity Framework Core |
| Database  | SQLite (`books.db`)                                 |
| Auth      | JWT Bearer tokens                                   |
| Frontend  | Angular 20 (standalone components, signals)         |
| Styling   | Bootstrap 5, SCSS, Font Awesome 7                   |
| Testing   | Jasmine + Karma (frontend)                          |

## API Endpoints

### Auth — `api/auth` (public)

| Method | Route                | Description          |
| ------ | -------------------- | -------------------- |
| POST   | `api/auth/register`  | Register a new user  |
| POST   | `api/auth/login`     | Log in, receive JWT  |

### Books — `api/books` (requires auth)

| Method | Route             | Description        |
| ------ | ----------------- | ------------------ |
| GET    | `api/books`       | List all books     |
| GET    | `api/books/{id}`  | Get a single book  |
| POST   | `api/books`       | Create a book      |
| PUT    | `api/books/{id}`  | Update a book      |
| DELETE | `api/books/{id}`  | Delete a book      |

### Citations — `api/my-citations` (requires auth, user-scoped)

| Method | Route                   | Description                |
| ------ | ----------------------- | -------------------------- |
| GET    | `api/my-citations`      | List your citations        |
| GET    | `api/my-citations/{id}` | Get one of your citations  |
| POST   | `api/my-citations`      | Create a citation          |
| PUT    | `api/my-citations/{id}` | Update your citation       |
| DELETE | `api/my-citations/{id}` | Delete your citation       |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) 18+ and npm
- [Angular CLI](https://angular.dev/tools/cli) (`npm install -g @angular/cli`)

## Getting Started

### 1. Run the API

```bash
cd RedRiverTest.Api
dotnet run
```

The API starts on `http://localhost:5268`. In development, the Scalar API docs are available at `http://localhost:5268/scalar`.

### 2. Run the Angular client

```bash
cd client
npm install
ng serve
```

Open [http://localhost:4200](http://localhost:4200) in your browser.

## Project Structure

```
RedRiverTest.Api/               # .NET 9 Web API
├── Controllers/
│   ├── AuthController.cs       # Register & login endpoints
│   ├── BooksController.cs      # Book CRUD (all users)
│   └── CitationsController.cs  # Citation CRUD (per user)
├── Models/
│   ├── Book.cs
│   └── Citation.cs
├── Auth/                       # JWT options, token service, user store
├── Data/
│   └── AppDbContext.cs         # EF Core context (SQLite)
├── Program.cs                  # App configuration & middleware
└── appsettings.json

client/                         # Angular 20 SPA
├── src/app/
│   ├── app.routes.ts           # Lazy-loaded route definitions
│   ├── guards/auth.guard.ts    # Route guard for protected pages
│   ├── interceptors/           # JWT HTTP interceptor
│   ├── services/               # Auth, book, citation, theme services
│   ├── home/                   # Book list page
│   ├── book-form/              # Add / edit book
│   ├── my-citations/           # Citation list with inline editing
│   ├── login/                  # Login page
│   └── register/               # Registration page
└── package.json
```

## Configuration

JWT settings are in `appsettings.Development.json` for local development. For production, set these as environment variables:

| Variable             | Description                       |
| -------------------- | --------------------------------- |
| `Jwt__SigningKey`    | A strong random 32+ character key |
| `Jwt__Issuer`       | `RedRiverTest`                    |
| `Jwt__Audience`     | `RedRiverTest`                    |
| `AllowedOrigins__0` | Your frontend URL                 |
