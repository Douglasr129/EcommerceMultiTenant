export interface UserPayload {
  id: string;
  email: string;
  userName: string;
  role: 'ADMIN' | 'USER' | 'MANAGER' | 'CLIENT' | 'SELLER'; // Adicionei os que você usou no register
}
