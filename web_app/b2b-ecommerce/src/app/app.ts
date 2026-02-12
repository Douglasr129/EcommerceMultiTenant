import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from "./layout/default/header/header";
import { Sidebar } from "./layout/default/sidebar/sidebar";
import { Footer } from "./layout/default/footer/footer";
import { AuthService } from './core/services/auth.service';
import { ToastsContainerComponent } from "./shared/components/toasts-container/toasts-container";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Sidebar, Footer, ToastsContainerComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  role: string = 'Admin';
  protected title = 'b2b-ecommerce';
  authService = inject(AuthService)
  ngOnInit(): void {
    this.role = this.authService.getRole();
  }
}
