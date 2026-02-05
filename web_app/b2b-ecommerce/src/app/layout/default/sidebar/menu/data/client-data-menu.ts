import { IMenu } from "../Interfaces/menu-interface";

export const CLIENT_DATA: IMenu[] = [

  // --- SEÇÃO CLIENTE (COMPRADOR B2B) ---
  {
    title: 'Comprar',
    icon: 'bi bi-shop',
    role: 'Client',
    children: [
      { title: 'Catálogo de Produtos', link: '/shop/catalog', role: 'Client' },
      { title: 'Listas de Compra (Recorrência)', link: '/shop/lists', role: 'Client' },
      { title: 'Pedidos Programados', link: '/shop/scheduled', role: 'Client' }
    ]
  },
  {
    title: 'Financeiro',
    icon: 'bi bi-cash-stack',
    role: 'Client',
    children: [
      { title: 'Meus Títulos / Boletos', link: '/finance/invoices', role: 'Client' },
      { title: 'Limite de Crédito', link: '/finance/credit', role: 'Client' },
      { title: 'Notas Fiscais', link: '/finance/tax-documents', role: 'Client' }
    ]
  },
];
