import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  NgbAccordionDirective,
  NgbAccordionModule,
  NgbAlertModule,
  NgbCollapse,
} from '@ng-bootstrap/ng-bootstrap';
import { IMenu } from '../Interfaces/menu-interface';

@Component({
  selector: 'app-nav-item',
  imports: [NgbAlertModule, NgbAccordionModule, NgbCollapse, RouterLink],
  templateUrl: './nav-item.html',
  styleUrl: './nav-item.scss',
})
export class NavItem implements OnChanges {
  @Input() item: IMenu | any;
  @Input() index: number = 0;
  @Input() isCollapsed: boolean = false;
  @Output() onExpand = new EventEmitter<void>();

  // Captura a instância do acordeão que está no HTML deste componente
  @ViewChild(NgbAccordionDirective) accordion!: NgbAccordionDirective;

  ngOnChanges(changes: SimpleChanges) {
    // Se isCollapsed mudou para TRUE, fechamos todos os itens deste nível
    if (changes['isCollapsed']?.currentValue === true && this.accordion) {
      this.accordion.collapseAll();
    }
  }
  // Método para tratar o clique
  handleItemClick() {
    if (this.isCollapsed) {
      this.onExpand.emit(); // Avisa o pai para abrir
    }
  }
}
