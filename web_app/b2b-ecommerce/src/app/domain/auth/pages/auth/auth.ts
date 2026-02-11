import { Component } from '@angular/core';
import { Login } from "../login/login";
import { Register } from "../register/register";

@Component({
  selector: 'app-auth',
  imports: [Login, Register],
  templateUrl: './auth.html',
  styleUrl: './auth.scss',
})
export class Auth {
  showRegister: boolean = false;

  toggleSlide() {
    this.showRegister = !this.showRegister;
  }
}
