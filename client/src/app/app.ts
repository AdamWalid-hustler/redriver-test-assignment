import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { AuthService } from './services/auth.service';
import { ThemeService } from './services/theme.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('RedRiverTest');
  protected auth = inject(AuthService);
  protected themeService = inject(ThemeService);
  protected navOpen = false;
}
