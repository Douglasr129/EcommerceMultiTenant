export const environment = {
  production: false,
  // Centralizamos as URLs dos microserviços aqui
  apis: {
    auth: 'http://localhost:3001/api/v1',
    products: 'http://localhost:3002/api/v1',
    orders: 'http://localhost:3003/api/v1',
    customers: 'http://localhost:3004/api/v1'
  }
};
