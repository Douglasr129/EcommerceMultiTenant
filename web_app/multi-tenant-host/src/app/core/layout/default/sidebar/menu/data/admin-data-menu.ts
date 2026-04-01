import { IMenu } from "../Interfaces/menu-interface";

export const ADMIN_DATA: IMenu[] = [
  // --- SEÇÃO ADMINISTRATIVA & GERENCIAL ---
  {
    title: 'Dashboard Estratégico',
    icon: 'bi bi-speedometer2',
    role: 'MANAGER',
    children: [
      { title: 'Visão Geral', link: '/dashboard/geral', role: 'MANAGER' },
      { title: 'Performance de Vendedores', link: '/reports/SELLERs', role: 'MANAGER' },
      { title: 'Análise de Margem', link: '/reports/margin', role: 'ADMIN' }
    ]
  },
  {
    title: 'Gestão de Usuários',
    icon: 'bi bi-people',
    role: 'ADMIN',
    children: [
      { title: 'Equipe Interna', link: '/ADMIN/users', role: 'ADMIN' },
      { title: 'Gerenciar CLIENTes (B2B)', link: '/ADMIN/CLIENTs', role: 'ADMIN' },
      { title: 'Aprovação de Cadastros', link: '/ADMIN/approvals', role: 'ADMIN' }
    ]
  },

  // --- SEÇÃO COMERCIAL (VENDEDOR) ---
  {
    title: 'Minhas Vendas',
    icon: 'bi bi-cart-check',
    role: 'SELLER',
    children: [
      { title: 'Novo Pedido', link: '/sales/new', role: 'SELLER' },
      { title: 'Carteira de CLIENTes', link: '/sales/my-CLIENTs', role: 'SELLER' },
      { title: 'Cotações em Aberto', link: '/sales/quotes', role: 'SELLER' },
      { title: 'Metas e Comissões', link: '/sales/metrics', role: 'SELLER' }
    ]
  },

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

  // --- LOGÍSTICA E ESTOQUE ---
  {
    title: 'Catálogo & Estoque',
    icon: 'bi bi-box-seam',
    role: 'MANAGER',
    children: [
      {
        title: 'Almoxarifado',
        icon: 'bi bi-house-gear',
        role: 'MANAGER',
        children: [
          { title: 'Entrada de Mercadoria', link: '/inventory/inbound', role: 'MANAGER' },
          { title: 'Saída/Expedição', link: '/inventory/outbound', role: 'MANAGER' },
          { title: 'Kits de Produtos', link: '/inventory/kits', role: 'MANAGER' }
        ]
      },
      { title: 'Tabelas de Preços', link: '/inventory/pricing', role: 'ADMIN' }
    ]
  },

  // --- CONFIGURAÇÕES GERAIS ---
  {
    title: 'Configurações',
    icon: 'bi bi-gear',
    role: 'ADMIN',
    children: [
      { title: 'Integração ERP', link: '/settings/erp', role: 'ADMIN' },
      { title: 'Regras de Frete', link: '/settings/shipping', role: 'ADMIN' },
      { title: 'Logs do Sistema', link: '/settings/logs', role: 'ADMIN' }
    ]
  }
];
