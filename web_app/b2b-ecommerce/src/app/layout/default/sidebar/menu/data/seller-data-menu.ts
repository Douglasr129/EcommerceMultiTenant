import { IMenu } from "../Interfaces/menu-interface";

export const SELLER_DATA: IMenu[] = [
  // --- SEÇÃO COMERCIAL (VENDEDOR) ---
  {
    title: 'Minhas Vendas',
    icon: 'bi bi-cart-check',
    role: 'Seller',
    children: [
      { title: 'Novo Pedido', link: '/sales/new', role: 'Seller' },
      { title: 'Carteira de Clientes', link: '/sales/my-clients', role: 'Seller' },
      { title: 'Cotações em Aberto', link: '/sales/quotes', role: 'Seller' },
      { title: 'Metas e Comissões', link: '/sales/metrics', role: 'Seller' }
    ]
  },
];
