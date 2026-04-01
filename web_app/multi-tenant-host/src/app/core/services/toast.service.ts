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
  showSuccess(message: string, header?: string) {
    this.show(message, { classname: 'custon-success', delay: 5000, header });
  }
  showError(message: string, header?: string) {
    this.show(message, { classname: 'custon-danger', delay: 8000, header });
  }
  showInfo(message: string, header?: string) {
    this.show(message, { classname: 'custon-info', delay: 8000, header });
  }
  show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTpl, ...options });
  }
  remove(toast: ToastInfo) {
    this.toasts = this.toasts.filter((t) => t !== toast);
  }
  clear() {
    this.toasts = [];
  }
}
