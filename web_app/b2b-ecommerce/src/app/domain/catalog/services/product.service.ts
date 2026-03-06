import { Injectable } from '@angular/core';
import { BaseService } from '../../../core/services/base.service';
import { catchError, map, Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductService extends BaseService {
  private readonly endpoint = `${this.urlBase}/products`;

  /**
   * Obtém a lista de produtos com filtros, paginação e ordenação
   */
  getAll(filters: any): Observable<any> {
    let params = new HttpParams()
      .set('page', filters.page.toString())
      .set('pageSize', filters.pageSize.toString())
      .set('sort', filters.sort)
      .set('order', filters.order)
      .set('showInactive', filters.showInactive.toString());

    if (filters.searchTerm) {
      params = params.set('searchTerm', filters.searchTerm);
    }
    if (filters.startDate) {
      params = params.set('startDate', filters.startDate);
    }
    if (filters.endDate) {
      params = params.set('endDate', filters.endDate);
    }

    return this.http
      .get(this.endpoint, {
        params,
        ...this.getHttpOptionsAuth() // Usa o método da BaseService para o Token
      })
      .pipe(
        map(this.extractData),
        catchError((err) => this.serviceError(err)) // Usa o tratamento de erro da BaseService
      );
  }

  /**
   * Busca um produto específico
   */
  getById(id: string): Observable<any> {
    return this.http
      .get(`${this.endpoint}/${id}`, this.getHttpOptionsAuth())
      .pipe(
        map(this.extractData),
        catchError((err) => this.serviceError(err))
      );
  }

  /**
   * Alterna o status (Ativo/Inativo)
   */
  toggleStatus(id: string): Observable<any> {
    return this.http
      .patch(`${this.endpoint}/${id}/toggle-status`, {}, this.getHttpOptionsAuth())
      .pipe(
        map(this.extractData),
        catchError((err) => this.serviceError(err))
      );
  }

  /**
   * Cria ou Atualiza um produto
   */
  save(product: any): Observable<any> {
    if (product.id) {
      return this.http
        .put(`${this.endpoint}/${product.id}`, product, this.getHttpOptionsAuth())
        .pipe(
          map(this.extractData),
          catchError((err) => this.serviceError(err))
        );
    }

    return this.http
      .post(this.endpoint, product, this.getHttpOptionsAuth())
      .pipe(
        map(this.extractData),
        catchError((err) => this.serviceError(err))
      );
  }
}
