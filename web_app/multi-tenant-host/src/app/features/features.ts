import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { ToastsContainerComponent } from "../shared/components/toasts-container/toasts-container";
import { Header } from "../core/layout/default/header/header";
import { Sidebar } from "../core/layout/default/sidebar/sidebar";
import { Footer } from "../core/layout/default/footer/footer";

@Component({
  selector: 'app-features',
  imports: [RouterOutlet, ToastsContainerComponent, Header, Sidebar, Footer],
  template: `

  <app-header></app-header>

  <div class="app-container">
    <app-sidebar></app-sidebar>
    <main class="app-content">
      <router-outlet></router-outlet>
    </main>
  </div>
  <app-footer></app-footer>
  <div class="app-container">
    <main class="app-content">
      <router-outlet></router-outlet>
    </main>
  </div>
<app-toasts-container></app-toasts-container>
  `,

})
export class Features {

}
