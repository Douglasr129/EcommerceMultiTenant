import { IMenu } from "../Interfaces/menu-interface";

export const ADMIN_DATA: IMenu[] = [
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
  {
    title: 'Gestão de Usuários',
    icon: 'bi bi-people',
    role: 'Admin',
    children: [
      { title: 'Equipe Interna', link: '/admin/users', role: 'Admin' },
      { title: 'Gerenciar Clientes (B2B)', link: '/admin/clients', role: 'Admin' },
      { title: 'Aprovação de Cadastros', link: '/admin/approvals', role: 'Admin' }
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

  // --- CONFIGURAÇÕES GERAIS ---
  {
    title: 'Configurações',
    icon: 'bi bi-gear',
    role: 'Admin',
    children: [
      { title: 'Integração ERP', link: '/settings/erp', role: 'Admin' },
      { title: 'Regras de Frete', link: '/settings/shipping', role: 'Admin' },
      { title: 'Logs do Sistema', link: '/settings/logs', role: 'Admin' }
    ]
  }
];
