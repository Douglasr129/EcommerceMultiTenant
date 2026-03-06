import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { CommonModule, UpperCasePipe } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { AppStateService } from '../../../../core/services/app-states.service';

@Component({
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  showPassword: boolean = false;
  showPasswordConfim: boolean = false;
  userRole: 'CLIENT' | 'SELLER' = 'CLIENT';

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private states = inject(AppStateService);

  toggleRole() {
    this.userRole = this.userRole === 'CLIENT' ? 'SELLER' : 'CLIENT';
    this.registerForm.patchValue({ role: this.userRole });
  }

  registerForm: FormGroup = this.fb.group(
    {
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
      role: ['CLIENT'],
    },
    {
      validators: this.passwordMatchValidator,
    },
  );

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { mismatch: true };
    }
    return null;
  }
  isFieldInvalid(field: string): boolean {
    const control = this.registerForm.get(field);
    return !!(control && control.invalid && (control.touched || control.dirty));
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
        this.authService.toast.showSuccess(
          'Conta criada com sucesso!',
          `Bem vindo ${res.user.userName}!`,
        );
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
        this.authService.toast.showError(
          `O correu um erro não tratado: ${err.message}!`,
          'Erro!',
        );
        console.log(err)
      },
    });
  }
}
