import { IMenu } from "../Interfaces/menu-interface";

export const MANAGER_DATA: IMenu[] = [
  // --- SEÇÃO ADMINISTRATIVA & GERENCIAL ---
  {
    title: 'Dashboard Estratégico',
    icon: 'bi bi-speedometer2',
    role: 'Manager',
    children: [
      { title: 'Visão Geral', link: '/dashboard/geral', role: 'Manager' },
      { title: 'Performance de Vendedores', link: '/reports/sellers', role: 'Manager' },
      { title: 'Análise de Margem', link: '/reports/margin', role: 'Admin' }
    ]
  },

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
  // --- LOGÍSTICA E ESTOQUE ---
  {
    title: 'Catálogo & Estoque',
    icon: 'bi bi-box-seam',
    role: 'Manager',
    children: [
      {
        title: 'Almoxarifado',
        icon: 'bi bi-house-gear',
        role: 'Manager',
        children: [
          { title: 'Entrada de Mercadoria', link: '/inventory/inbound', role: 'Manager' },
          { title: 'Saída/Expedição', link: '/inventory/outbound', role: 'Manager' },
          { title: 'Kits de Produtos', link: '/inventory/kits', role: 'Manager' }
        ]
      },
      { title: 'Tabelas de Preços', link: '/inventory/pricing', role: 'Admin' }
    ]
  },
];
