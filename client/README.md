# RedRiverTest — Book & Citation Manager

A full-stack single-page application built with **Angular 20** and **Bootstrap 5** for managing books and personal citations. Users can register, log in, and perform full CRUD operations on books and citations. Built as part of the Red River internship acceptance project.

## Features

- **JWT Authentication** — Register and log in with token-based auth; protected routes via an Angular route guard and HTTP interceptor.
- **Book Management** — View, add, edit, and delete books (title, author, published date).
- **My Citations** — Save, edit, and delete personal quotes/citations. Comes with default starter quotes.
- **Dark / Light Theme** — Toggle between themes with automatic preference detection and persistence via `localStorage`.
- **Lazy-Loaded Routes** — Every page is lazy-loaded for fast initial load times.
- **Responsive UI** — Bootstrap 5 grid and components with Font Awesome icons.

## Tech Stack

| Layer       | Technology                        |
| ----------- | --------------------------------- |
| Framework   | Angular 20 (standalone components, signals) |
| Styling     | Bootstrap 5, SCSS                 |
| Icons       | Font Awesome 7                    |
| HTTP        | Angular HttpClient + JWT interceptor |
| Backend API | .NET Web API (separate project, port 5268) |
| Testing     | Jasmine + Karma                   |

## Prerequisites

- **Node.js** 18+ and **npm**
- **Angular CLI** (`npm install -g @angular/cli`)
- The **.NET backend API** running on `http://localhost:5268` (see the parent `RedRiverTest.Api` project)

## Getting Started

```bash
# Install dependencies
npm install

# Start the dev server
ng serve
```

Open [http://localhost:4200](http://localhost:4200) in your browser. The app will hot-reload on file changes.

## Available Scripts

| Command         | Description                              |
| --------------- | ---------------------------------------- |
| `npm start`     | Start the development server             |
| `npm test`      | Run unit tests with Karma                |
| `npm run build` | Production build to `dist/`              |
| `npm run watch` | Build in watch mode (development)        |

## Project Structure

```
src/app/
├── app.ts / app.html          # Root component with navbar
├── app.routes.ts               # Route definitions (lazy-loaded)
├── app.config.ts               # App-level providers (router, HTTP, interceptors)
├── guards/
│   └── auth.guard.ts           # Redirects unauthenticated users to /login
├── interceptors/
│   └── auth.interceptor.ts     # Attaches JWT token to outgoing requests
├── services/
│   ├── auth.service.ts         # Login, register, logout, token management
│   ├── book.service.ts         # Book CRUD operations
│   ├── citations.service.ts    # Citation CRUD operations
│   └── theme.service.ts        # Dark/light theme toggle
├── home/                       # Book list (home page)
├── book-form/                  # Add / edit book form
├── my-citations/               # Citation list with inline add/edit
├── login/                      # Login page
└── register/                   # Registration page
```
