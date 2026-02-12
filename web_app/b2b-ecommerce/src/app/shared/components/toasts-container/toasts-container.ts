import { Component, inject, TemplateRef } from '@angular/core';
import { ToastService, ToastInfo } from '../../../core/services/toast.service';
import { NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-toasts-container',
  standalone: true,
  imports: [NgbToastModule, NgTemplateOutlet],
  template: `
    @for (toast of toastService.toasts; track toast) {
      <ngb-toast
        [class]="toast.classname"
        [autohide]="true"
        [delay]="toast.delay || 5000"
        (hidden)="toastService.remove(toast)"
      >
        @if (toast.header) {
          <ng-template ngbToastHeader>
            <strong class="me-auto">{{ toast.header }}</strong>
          </ng-template>
        }

        @if (isTemplate(toast)) {
          <ng-container [ngTemplateOutlet]="asTemplate(toast.textOrTpl)"></ng-container>
        } @else {
          {{ toast.textOrTpl }}
        }
      </ngb-toast>
    }
  `,
  host: {
    class: 'toast-container position-fixed top-0 end-0 p-3',
    style: 'z-index: 1200'
  },
})
export class ToastsContainerComponent {
  toastService = inject(ToastService);

  // Verifica se o conteúdo é um TemplateRef
  isTemplate(toast: ToastInfo): boolean {
    return toast.textOrTpl instanceof TemplateRef;
  }

  // Helper para fazer o "cast" do tipo para o HTML não reclamar
  asTemplate(val: any): TemplateRef<any> {
    return val as TemplateRef<any>;
  }
}
