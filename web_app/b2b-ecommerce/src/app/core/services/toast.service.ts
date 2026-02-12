import { Injectable, TemplateRef } from '@angular/core';

export interface ToastInfo {
  textOrTpl: string | TemplateRef<any>;
  classname?: string;
  delay?: number;
  header?: string;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  toasts: ToastInfo[] = [];

  // Exibe um toast de sucesso
  showSuccess(message: string, header?: string) {
    this.show(message, { classname: 'bg-success text-light', delay: 5000, header });
  }

  // Exibe um toast de erro
  showError(message: string, header?: string) {
    this.show(message, { classname: 'bg-danger text-light', delay: 8000, header });
  }

  // Método base
  show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTpl, ...options });
  }

  // Remove o toast da lista
  remove(toast: ToastInfo) {
    this.toasts = this.toasts.filter((t) => t !== toast);
  }

  clear() {
    this.toasts = [];
  }
}
