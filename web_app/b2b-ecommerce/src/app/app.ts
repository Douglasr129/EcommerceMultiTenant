import {
  Component,
  computed,
  effect,
  inject,
  PLATFORM_ID,
} from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Header } from './layout/default/header/header';
import { Sidebar } from './layout/default/sidebar/sidebar';
import { Footer } from './layout/default/footer/footer';
import { AuthService } from './core/services/auth.service';
import { ToastsContainerComponent } from './shared/components/toasts-container/toasts-container';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, map } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { AppStateService } from './core/services/app-states.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Sidebar, Footer, ToastsContainerComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  public state = inject(AppStateService);
  private platformId = inject(PLATFORM_ID);
  private router = inject(Router);
  private currentUrl = toSignal(
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map((event: any) => event.urlAfterRedirects),
    ),
    { initialValue: this.router.url },
  );
  constructor() {
    effect(() => {
      const isDark = this.state.isDark();
      if (isPlatformBrowser(this.platformId)) {
        if (isDark) {
          document.body.classList.add('dark-theme');
          document.body.classList.remove('light-theme');
        } else {
          document.body.classList.add('light-theme');
          document.body.classList.remove('dark-theme');
        }
      }
    });
  }
  showLayout = computed(() => {
   const url = this.currentUrl();
    const publicRoutes = ['/', '/login', '/registrar', '/auth'];
    const isPublic = publicRoutes.includes(url);
    return !isPublic;
  });
}
