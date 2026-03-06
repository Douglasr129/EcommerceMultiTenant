import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { IMenu } from './menu/Interfaces/menu-interface';
import { ADMIN_DATA } from './menu/data/admin-data-menu';
import { MANAGER_DATA } from './menu/data/manager-data-menu';
import { SELLER_DATA } from './menu/data/seller-data-menu';
import { CLIENT_DATA } from './menu/data/client-data-menu';
import { NavItem } from "./menu/nav-item/nav-item";

@Component({
  selector: 'app-sidebar',
  imports: [NavItem],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar implements OnInit {
  role?: string | null;
  itensMenu: IMenu[] | any;
  toggleSidebar = signal(false);
  authService = inject(AuthService)

  ngOnInit(): void {
    this.role = this.authService.getUserRole();
    switch (this.role) {
      case 'ADMIN':
        this.itensMenu = ADMIN_DATA;
        break;
      case 'MANAGER':
        this.itensMenu = MANAGER_DATA;
        break;
      case 'SELLER':
        this.itensMenu = SELLER_DATA;
        break;
      case 'CLIENT':
        this.itensMenu = CLIENT_DATA;
        break;
    }
  }
  toggleHandle() {
    this.toggleSidebar.update((current) => !current);
  }
}
