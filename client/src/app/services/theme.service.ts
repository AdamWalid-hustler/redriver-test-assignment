import { Injectable, signal, effect } from "@angular/core";

export type Theme = 'light' | 'dark';

@Injectable({providedIn: 'root'})
export class ThemeService {
    readonly theme = signal<Theme>(this.getInitialTheme());

    constructor() {
        effect(() => {
            const t = this.theme();
            document.documentElement.setAttribute('data-bs-theme', t);
            localStorage.setItem('theme', t);
        });

    }

    toggle(): void {
        this.theme.update(current =>(current === 'light' ? 'dark' : 'light'));
    }

    private getInitialTheme(): Theme {
        const saved = localStorage.getItem('theme') as Theme | null;
        if  (saved === 'light' || saved === 'dark') return saved;

        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        return prefersDark ? 'dark' : 'light';
    }
}