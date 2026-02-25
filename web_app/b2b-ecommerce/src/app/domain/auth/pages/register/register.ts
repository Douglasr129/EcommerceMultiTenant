import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule, UpperCasePipe } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [CommonModule, UpperCasePipe, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  showPassword: boolean = false;
  showPasswordConfim: boolean = false;
  userRole: 'cliente' | 'vendedor' = 'cliente';

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router)

  toggleRole() {
    this.userRole = this.userRole === 'cliente' ? 'vendedor' : 'cliente';
  }

  registerForm = this.fb.nonNullable.group(
    {
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: this.passwordMatchValidator },
  );

  isFieldInvalid(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return !!(field?.touched && field?.invalid);
  }

  private passwordMatchValidator(group: any) {
    const pass = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return pass === confirm ? null : { mismatch: true };
  }
  onSubmit() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const { confirmPassword, ...rawValues } = this.registerForm.getRawValue();
    const payload = { ...rawValues, role: this.userRole };

    this.authService.register(payload).subscribe({
      next: (res) => {
        this.authService.showSuccess('Conta criada com sucesso!', `Bem vindo ${res.user.name}!`)
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.authService.showError('Erro!', `O correu um erro não tratado: ${err}!`)
      },
    });
  }
}
