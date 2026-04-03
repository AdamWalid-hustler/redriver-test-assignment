import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

// Book type matching the API response
interface Book {
  id: number;
  title: string;
  author: string;
  publishedDate: string;
}

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styles: ``,
})
export class Home implements OnInit {
  private http = inject(HttpClient);
  books = signal<Book[]>([]);
  error = signal('');

  // Fetch books from API when page loads
  ngOnInit() {
    this.http.get<Book[]>('http://localhost:5268/api/books').subscribe({
      next: (data) => this.books.set(data),
      error: (err) => this.error.set(err.status === 401 ? 'Unauthorized – please log in' : 'Failed to load books'),
    });
  }
}
