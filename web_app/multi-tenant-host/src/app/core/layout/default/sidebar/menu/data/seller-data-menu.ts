import { IMenu } from '../Interfaces/menu-interface';

export const SELLER_DATA: IMenu[] = [
  // --- SEÇÃO COMERCIAL (VENDEDOR) ---
  {
    title: 'Vendas',
    icon: 'bi bi-cart-check',
    role: 'SELLER',
    children: [
      { title: 'Novo Pedido', link: '/catalog/new', role: 'SELLER' },
      {
        title: 'Carteira de Clientes',
        link: '/catalog/my-clients',
        role: 'SELLER',
      },
      { title: 'Cotações em Aberto', link: '/catalog/quotes', role: 'SELLER' },
      { title: 'Metas e Comissões', link: '/catalog/metrics', role: 'SELLER' },
    ],
  },
  {
      title: 'Gestão de Catálogo', // Agrupador principal
      icon: 'bi bi-grid-3x3-gap-fill',
      role: 'SELLER',
      children: [
        { title: 'Produtos', link: '/catalog/products', icon: 'bi bi-box-seam', role: 'SELLER' },
        { title: 'Categorias', link: '/catalog/categories', icon: 'bi bi-tags', role: 'SELLER' },
        { title: 'Chaves de Acesso', link: '/catalog/keys', icon: 'bi bi-key', role: 'SELLER' },
        { title: 'Ver Catálogo (Loja)', link: '/catalog/view', icon: 'bi bi-eye', role: 'SELLER' },
      ],
    },
];
