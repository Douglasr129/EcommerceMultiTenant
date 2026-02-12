import { Component } from '@angular/core';
import { Login } from "../login/login";
import { Register } from "../register/register";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-auth',
  imports: [Login, Register, RouterLink],
  templateUrl: './auth.html',
  styleUrl: './auth.scss',
})
export class Auth {
  showRegister: boolean = false;

  toggleSlide() {
    this.showRegister = !this.showRegister;
  }
}
