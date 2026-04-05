import { Component, inject, OnInit, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CitationService, Citation } from "../services/citations.service";

@Component({
    selector: 'app-my-citations',
    imports: [FormsModule],
    templateUrl: './my-citations.html',
    styles: ``,
})
export class Citations implements OnInit {
    private citationService = inject(CitationService);

    citations = signal<Citation[]>([]);
    error = signal('');
    loading = signal(false);

    // Form fields
    text = '';
    author = '';
    editId: number | null = null;

    get isEdit() {
        return this.editId !== null;
    }

    ngOnInit() {
        this.loadCitations();
    }

    loadCitations() {
        this.citationService.getAll().subscribe({
            next: (data) => this.citations.set(data),
            error: (err) => this.error.set(err.status === 401 ? 'Unauthorized - please log in' : 'Failed to load citations'),
        });
    }

    startEdit(citation: Citation) {
        this.editId = citation.id;
        this.text = citation.text;
        this.author = citation.author;
    }

    cancelEdit() {
        this.editId = null;
        this.text = '';
        this.author = '';
    }

    onSubmit() {
        this.error.set('');
        this.loading.set(true);

        const body = { text: this.text, author: this.author };

        const request = this.isEdit
            ? this.citationService.update(this.editId!, body)
            : this.citationService.create(body);

        request.subscribe({
            next: () => {
                this.loading.set(false);
                this.cancelEdit();
                this.loadCitations();
            },
            error: () => {
                this.loading.set(false);
                this.error.set('Failed to save citation');
            },
        });
    }

    deleteCitation(id: number) {
        if (!confirm('Are you sure you want to delete this citation?')) return;
        this.citationService.delete(id).subscribe({
            next: () => this.loadCitations(),
            error: () => this.error.set('Failed to delete citation'),
        });
    }
}
