export interface IMenu{
title: string;
  icon?: string;
  link?: string;
  role: 'Admin' | 'Manager' | 'Seller' | 'Client' | 'All';
  children?: IMenu[];
}
