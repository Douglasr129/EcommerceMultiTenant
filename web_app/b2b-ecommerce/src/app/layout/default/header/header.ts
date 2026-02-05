import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  isDarkTheme = signal(false);
isCollapsed: any;

  toggleTheme() {
    // 1. Inverte o valor do signal
    this.isDarkTheme.update((current) => !current);

    // 2. Aplica a classe no body
    if (this.isDarkTheme()) {
      document.body.classList.add('dark-theme');
    } else {
      document.body.classList.remove('dark-theme');
    }
  }
}
