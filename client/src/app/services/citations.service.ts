import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";

export interface Citation {
    id: number;
    text: string;
    author: string;
    userId: string;
}

// url for my citations
const API = 'https://redriver-test-assignment.onrender.com/api/my-citations';

@Injectable({ providedIn: 'root' })
export class CitationService {
    private http = inject(HttpClient);

    getAll() {
        return this.http.get<Citation[]>(API);
    }

    getById(id: number) {
        return this.http.get<Citation>(`${API}/${id}`);
    }

    create(citation: { text: string; author: string }) {
        return this.http.post<Citation>(API, citation);
    }

    update(id: number, citation: { text: string; author: string }) {
        return this.http.put<Citation>(`${API}/${id}`, citation);
    }

    delete(id: number) {
        return this.http.delete(`${API}/${id}`);
    }
}