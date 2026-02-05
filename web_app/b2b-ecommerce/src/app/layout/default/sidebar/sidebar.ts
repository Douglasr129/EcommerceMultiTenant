import { Component, OnInit, signal } from '@angular/core';
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
  role: string = 'Admin';
  itensMenu: IMenu[] | any;
  toggleSidebar = signal(false);

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.role = this.authService.getRole();
    this.role = 'Admin';

    switch (this.role) {
      case 'Admin':
        this.itensMenu = ADMIN_DATA;
        break;
      case 'Manager':
        this.itensMenu = MANAGER_DATA;
        break;
      case 'Seller':
        this.itensMenu = SELLER_DATA;
        break;
      case 'Client':
        this.itensMenu = CLIENT_DATA;
        break;
    }
  }
  toggleHandle() {
    this.toggleSidebar.update((current) => !current);
  }
}
