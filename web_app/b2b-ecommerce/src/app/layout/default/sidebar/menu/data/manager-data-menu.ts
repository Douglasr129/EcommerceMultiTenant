import { IMenu } from '../Interfaces/menu-interface';

export const MANAGER_DATA: IMenu[] = [
  // --- SEÇÃO ADMINISTRATIVA & GERENCIAL ---
  {
    title: 'Dashboard Estratégico',
    icon: 'bi bi-speedometer2',
    role: 'MANAGER',
    children: [
      { title: 'Visão Geral', link: '/dashboard/geral', role: 'MANAGER' },
      {
        title: 'Performance de Vendedores',
        link: '/reports/sellers',
        role: 'MANAGER',
      },
    ],
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
          {
            title: 'Entrada de Mercadoria',
            link: '/inventory/inbound',
            role: 'MANAGER',
          },
          {
            title: 'Saída/Expedição',
            link: '/inventory/outbound',
            role: 'MANAGER',
          },
          {
            title: 'Kits de Produtos',
            link: '/inventory/kits',
            role: 'MANAGER',
          },
        ],
      },
    ],
  },
];
