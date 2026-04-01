import {
  isPlatformBrowser,
  ViewportScroller,
  CommonModule,
} from '@angular/common';
import { Component, inject, PLATFORM_ID, HostListener } from '@angular/core';
import { RouterLink } from '@angular/router';
import { About } from '../about/about';
import { Contact } from '../contact/contact';
import { Features } from '../features/features';
import { AppStateService } from '../../core/services/app-states.service';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, About, Contact, Features, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
  public state = inject(AppStateService);
  private scroller = inject(ViewportScroller);
  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));
  private toast = inject(ToastService);
  isSidebarCollapsed = false;
  testeCookies() {
    var nome = this.state.userData()?.userName
    this.toast.showInfo(nome?nome:'Deu ruim')
  }
  activeCard: string = 'cliente';
  menuLinks = [
    { title: 'Início', fragment: 'inicio', icon: 'bi-house' },
    { title: 'Sobre', fragment: 'sobre', icon: 'bi-info-circle' },
    { title: 'Funcionalidades', fragment: 'features', icon: 'bi-star' },
    { title: 'Contato', fragment: 'contato', icon: 'bi-envelope' },
  ];
  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }
  scrollTo(fragment: string) {
    this.scroller.scrollToAnchor(fragment);
    if (this.isBrowser && window.innerWidth < 768) {
      this.isSidebarCollapsed = true;
    }
  }
  toggleTheme() {
    const THEME_MODE = this.state.isDark() ? 'light' : 'dark';
    this.state.updateTheme(THEME_MODE);
  }
  setActiveCard(type: string) {
    this.activeCard = type;
  }
}
