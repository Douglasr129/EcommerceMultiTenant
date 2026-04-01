import { Component, ElementRef, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-contact',
  imports: [ReactiveFormsModule],
  templateUrl: './contact.html',
  styleUrl: './contact.scss',
})
export class Contact {
private fb = inject(FormBuilder);
  private authService = inject(AuthService); // Injetado para usar o serviço de Toast/Notification

  public contactForm: FormGroup;
  loading = signal(false);

  constructor() {
    this.contactForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      cnpj: ['', [Validators.required, Validators.pattern(/^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/)]],
      email: ['', [Validators.required, Validators.email]],
      message: ['', [Validators.required, Validators.minLength(10)]],
    });
  }

  // Mantendo o mesmo nome de método do seu Login para consistência no HTML
  isFieldInvalid(field: string): boolean {
    const control = this.contactForm.get(field);
    return !!(control && control.invalid && (control.touched || control.dirty));
  }

  onContactSubmit() {
    if (this.contactForm.invalid) {
      this.authService.toast.showError(
        'Por favor, preencha todos os campos corretamente.',
        'Formulário Inválido'
      );
      return;
    }

    this.loading.set(true);

    // Simulando uma chamada para o seu backend .NET
    console.log('Dados do Contato:', this.contactForm.value);

    // Aqui você chamaria um método no seu service, ex: this.contactService.send(data)
    setTimeout(() => {
      this.authService.toast.showSuccess(
        'Sua solicitação foi enviada com sucesso! Entraremos em contato em breve.',
        'Enviado'
      );
      this.loading.set(false);
      this.contactForm.reset();
    }, 1500);
  }
}
