export interface IMenu{
title: string;
  icon?: string;
  link?: string;
  role: 'ADMIN' | 'MANAGER' | 'SELLER' | 'CLIENT' ;
  children?: IMenu[];
}
