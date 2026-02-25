import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ToastService } from './toast.service';

export abstract class BaseService extends ToastService {

  protected httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    })
  };
  protected extractData(response: any) {
    return response || {};
  }
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
    console.error(`[API Error ${error.status}]: ${errorMessage}`, error);

    return throwError(() => ({
      status: error.status,
      message: errorMessage,
      originalError: error
    }));
  }
}
