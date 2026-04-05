import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RouterLink } from '@angular/router';

const API = 'http://localhost:5268/api/books';

// Book type matching the API response
interface Book {
  id: number;
  title: string;
  author: string;
  publishedDate: string;
}

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styles: ``,
})
export class Home implements OnInit {
  private http = inject(HttpClient);
  books = signal<Book[]>([]);
  error = signal('');

  // Fetch books from API when page loads
  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.http.get<Book[]>(API).subscribe({
      next: (data) => this.books.set(data),
      error: (err) => this.error.set(err.status === 401 ? 'Unauthorized – please log in' : 'Failed to load books'),
    });
  }

  // Delete a book and refresh the list
  deleteBook(id: number) {
    if (!confirm('Are you sure you want to delete this book?')) return;
    this.http.delete(`${API}/${id}`).subscribe({
      next: () => this.loadBooks(),
      error: () => this.error.set('Failed to delete book'),
    });
  }
}
