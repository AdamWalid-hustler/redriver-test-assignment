import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BookService } from '../services/book.service';

@Component({
  selector: 'app-book-form',
  imports: [FormsModule, RouterLink],
  templateUrl: './book-form.html',
  styles: ``,
})
export class BookForm implements OnInit {
  private bookService = inject(BookService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  // Form fields
  title = '';
  author = '';
  publishedDate = '';

  error = signal('');
  loading = signal(false);

  // If editing, this will be the book id
  editId: number | null = null;

  // Check if we are editing or adding
  get isEdit() {
    return this.editId !== null;
  }

  ngOnInit() {
    // If the URL has an id param, load that book for editing
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.editId = +id;
      this.loading.set(true);
      this.bookService.getById(this.editId).subscribe({
        next: (book) => {
          this.title = book.title;
          this.author = book.author;
          // Format date for the input field (yyyy-MM-dd)
          this.publishedDate = book.publishedDate.substring(0, 10);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Could not load book');
          this.loading.set(false);
        },
      });
    }
  }

  onSubmit() {
    this.error.set('');
    this.loading.set(true);

    const body = {
      title: this.title,
      author: this.author,
      publishedDate: this.publishedDate,
    };

    // Use PUT for edit, POST for new
    const request = this.isEdit
      ? this.bookService.update(this.editId!, body)
      : this.bookService.create(body);

    request.subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/']);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Failed to save book');
      },
    });
  }
}
