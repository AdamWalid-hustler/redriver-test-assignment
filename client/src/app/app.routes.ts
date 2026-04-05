import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./home/home').then(m => m.Home), canActivate: [authGuard] },
  { path: 'books/add', loadComponent: () => import('./book-form/book-form').then(m => m.BookForm), canActivate: [authGuard] },
  { path: 'books/edit/:id', loadComponent: () => import('./book-form/book-form').then(m => m.BookForm), canActivate: [authGuard] },
  { path: 'login', loadComponent: () => import('./login/login').then(m => m.Login) },
  { path: 'register', loadComponent: () => import('./register/register').then(m => m.Register) },
  { path: 'my-citations', loadComponent: () => import('./my-citations/my-citations').then(m => m.Citations), canActivate: [authGuard] },
  { path: '**', redirectTo: '' },
];
