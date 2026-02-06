import { ViewportScroller } from '@angular/common';
import { Component } from '@angular/core';
import { About } from '../about/about';
import { Contact } from '../contact/contact';
import { Features } from '../features/features';
import {
  NgbDropdown,
  NgbDropdownButtonItem,
  NgbDropdownItem,
  NgbDropdownMenu,
  NgbDropdownToggle,
} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home',
  imports: [
    About,
    Contact,
    Features,
    NgbDropdown,
    NgbDropdownToggle,
    NgbDropdownMenu,
    NgbDropdownItem,
    NgbDropdownButtonItem,
  ],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
  isSidebarCollapsed = false;
  isDarkTheme = false;
  activeCard: string = 'cliente';
  // Menu definido internamente
  menuLinks = [
    { title: 'Início', fragment: 'inicio', icon: 'bi-house' },
    { title: 'Sobre', fragment: 'sobre', icon: 'bi-info-circle' },
    { title: 'Funcionalidades', fragment: 'features', icon: 'bi-star' },
    { title: 'Contato', fragment: 'contato', icon: 'bi-envelope' },
  ];

  constructor(private scroller: ViewportScroller) {}

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  scrollTo(fragment: string) {
    this.scroller.scrollToAnchor(fragment);
    // Opcional: fechar ao clicar em telas pequenas
    if (window.innerWidth < 768) this.isSidebarCollapsed = true;
  }
  toggleTheme() {
    this.isDarkTheme = !this.isDarkTheme;
    // Opcional: Salvar no localStorage para persistir
    document.body.classList.toggle('dark-theme', this.isDarkTheme);
  }
  setActiveCard(type: string) {
    this.activeCard = type;
  }
}
