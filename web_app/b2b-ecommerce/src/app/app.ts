import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from "./layout/default/header/header";
import { Sidebar } from "./layout/default/sidebar/sidebar";
import { Footer } from "./layout/default/footer/footer";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Sidebar, Footer],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'b2b-ecommerce';
}
