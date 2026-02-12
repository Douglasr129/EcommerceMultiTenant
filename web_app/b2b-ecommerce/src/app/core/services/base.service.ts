import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';

export abstract class BaseService {

  protected httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    })
  };

  /**
   * Extrai os dados da resposta (útil para logs ou manipulações genéricas)
   */
  protected extractData(response: any) {
    return response || {};
  }

  /**
   * Tratamento centralizado de erros HTTP
   */
  protected serviceError(error: HttpErrorResponse) {
    let errorMessage = '';

    if (error.status === 0) {
      // Erro de rede ou CORS
      errorMessage = 'Falha na conexão com o servidor. Verifique sua internet.';
    } else if (error.status >= 400 && error.status < 500) {
      // Erros do cliente (400, 401, 403, 404)
      errorMessage = error.error?.message || error.error?.errors?.[0] || 'Dados inválidos ou acesso negado.';
    } else {
      // Erros do servidor (500, 503)
      errorMessage = 'Ocorreu um erro interno no servidor. Tente novamente mais tarde.';
    }

    // Aqui você poderia injetar um serviço de Notificação (Toast) global
    console.error(`[API Error ${error.status}]: ${errorMessage}`, error);

    return throwError(() => ({
      status: error.status,
      message: errorMessage,
      originalError: error
    }));
  }
}
