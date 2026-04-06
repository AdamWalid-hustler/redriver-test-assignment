import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BookService, Book } from '../services/book.service';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styles: ``,
})
export class Home implements OnInit {
  private bookService = inject(BookService);
  books = signal<Book[]>([]);
  error = signal('');

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.bookService.getAll().subscribe({
      next: (data) => this.books.set(data),
      error: (err) => this.error.set(err.status === 401 ? 'Unauthorized – please log in' : 'Failed to load books'),
    });
  }

  deleteBook(id: number) {
    if (!confirm('Are you sure you want to delete this book?')) return;
    this.bookService.delete(id).subscribe({
      next: () => this.loadBooks(),
      error: () => this.error.set('Failed to delete book'),
    });
  }
}
