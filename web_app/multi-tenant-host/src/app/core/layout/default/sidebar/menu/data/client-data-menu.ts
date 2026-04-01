import { IMenu } from "../Interfaces/menu-interface";

export const CLIENT_DATA: IMenu[] = [

  // --- SEÇÃO CLIENTE (COMPRADOR B2B) ---
  {
    title: 'Comprar',
    icon: 'bi bi-shop',
    role: 'CLIENT',
    children: [
      { title: 'Catálogo de Produtos', link: '/shop/catalog', role: 'CLIENT' },
      { title: 'Listas de Compra (Recorrência)', link: '/shop/lists', role: 'CLIENT' },
      { title: 'Pedidos Programados', link: '/shop/scheduled', role: 'CLIENT' }
    ]
  },
  {
    title: 'Financeiro',
    icon: 'bi bi-cash-stack',
    role: 'CLIENT',
    children: [
      { title: 'Meus Títulos / Boletos', link: '/finance/invoices', role: 'CLIENT' },
      { title: 'Limite de Crédito', link: '/finance/credit', role: 'CLIENT' },
      { title: 'Notas Fiscais', link: '/finance/tax-documents', role: 'CLIENT' }
    ]
  },
];
