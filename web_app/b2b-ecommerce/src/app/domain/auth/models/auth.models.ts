export interface AuthResponse {
  token: string;
  user: User;
}

export interface User {
  id: string;
  name: string;
  email: string;
  role: 'cliente' | 'vendedor';
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  role: 'cliente' | 'vendedor';
}
