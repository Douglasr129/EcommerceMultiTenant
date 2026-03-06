import { Component, computed, effect, inject, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-produtos',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './produtos.html',
  styleUrl: './produtos.scss',
})
export class Produtos {
private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private router = inject(Router);
  public filters = computed(() => this.filterForm.value);
  // --- Signals de Estado ---
  products = signal<any[]>([]);
  totalItems = signal(0);
  currentPage = signal(1);
  pageSize = signal(10);
  loading = signal(false);
  sortField = signal('name');
  sortOrder = signal<'asc' | 'desc'>('asc');

  // --- Formulário de Filtros ---
  filterForm = this.fb.group({
    searchTerm: [''],
    startDate: [''],
    endDate: [''],
    showInactive: [false]
  });

  // --- Computed para Paginação ---
  totalPages = computed(() => Math.ceil(this.totalItems() / this.pageSize()));

  constructor() {
    // Efeito: Sempre que a página ou ordenação mudar, busca os dados
    effect(() => {
      this.loadProducts();
    }, { allowSignalWrites: true });
  }

  loadProducts() {
    this.loading.set(true);

    const params = {
      page: this.currentPage(),
      pageSize: this.pageSize(),
      sort: this.sortField(),
      order: this.sortOrder(),
      ...this.filterForm.value
    };

    this.productService.getAll(params).subscribe({
      next: (response: any) => {
        this.products.set(response.data);
        this.totalItems.set(response.total);
        this.loading.set(false);
      },
      error: () => {
        this.productService.toast.showError('Erro ao carregar produtos');
        this.loading.set(false);
      }
    });
  }

  // --- Ações de Filtro e Ordenação ---
  onSearch() {
    this.currentPage.set(1); // Reseta para a primeira página ao pesquisar
    this.loadProducts();
  }

  sort(field: string) {
    if (this.sortField() === field) {
      this.sortOrder.set(this.sortOrder() === 'asc' ? 'desc' : 'asc');
    } else {
      this.sortField.set(field);
      this.sortOrder.set('asc');
    }
  }

  toggleStatus() {
    const currentValue = this.filterForm.get('showInactive')?.value;
    this.filterForm.patchValue({ showInactive: !currentValue });
    this.onSearch();
  }

  // --- Navegação e Comandos ---
  novoProduto() {
    this.router.navigate(['/products/new']);
  }

  edit(id: string) {
    this.router.navigate(['/products/edit', id]);
  }

  view(id: string) {
    this.router.navigate(['/products/view', id]);
  }

  toggleActive(product: any) {
    const action = product.active ? 'inativar' : 'ativar';

    this.productService.toggleStatus(product.id).subscribe({
      next: () => {
        this.productService.toast.showInfo(`Produto ${product.name} foi ${action}do.`);
        this.loadProducts();
      },
      error: () => this.productService.toast.showError(`Erro ao ${action} o produto.`)
    });
  }

  changePage(delta: number) {
    const next = this.currentPage() + delta;
    if (next >= 1 && next <= this.totalPages()) {
      this.currentPage.set(next);
    }
  }
}
