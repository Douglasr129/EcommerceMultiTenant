import { Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';
import { AppStateService } from '../../../../core/services/app-states.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private states = inject(AppStateService);
  loginForm: FormGroup;
  showPassword = false;
  loading = signal(false);

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  isFieldInvalid(field: string): boolean {
    const control = this.loginForm.get(field);
    return !!(control && control.invalid && (control.touched || control.dirty));
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.authService.toast.showError(
        'Por favor, preencha os campos corretamente.',
        'Erro',
      );
      return;
    }

    this.loading.set(true);

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.authService.toast.showSuccess(
          `Bem-vindo de volta, ${res.user.userName}!`,
          'Sucesso',
        );
        if (res.user.role !== '')
          switch (res.user.role) {
            case 'SELLER':
              this.router.navigate(['/seller/']);
              break;
            default:
              this.router.navigate(['/client/']);
              break;
          }
      },
      error: (err) => {
        this.loading.set(false);
        const message = err.error?.message || 'E-mail ou senha incorretos.';
        this.authService.toast.showError(message, 'Erro');
      },
      complete: () => this.loading.set(false),
    });
  }
}
