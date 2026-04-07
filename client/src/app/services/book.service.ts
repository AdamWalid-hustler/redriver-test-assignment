import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";

export interface Book {
    id: number;
    title: string;
    author: string;
    publishedDate: string;
}

const API = 'https://redriver-test-assignment.onrender.com/api/books';

@Injectable({ providedIn: 'root' })
export class BookService {
    private http = inject(HttpClient);

    getAll() {
        return this.http.get<Book[]>(API);
    }

    getById(id: number) {
        return this.http.get<Book>(`${API}/${id}`);
    }

    create(book: { title: string; author: string; publishedDate: string }) {
        return this.http.post<Book>(API, book);
    }

    update(id: number, book: { title: string; author: string; publishedDate: string }) {
        return this.http.put<Book>(`${API}/${id}`, book);
    }

    delete(id: number) {
        return this.http.delete(`${API}/${id}`);
    }
}
