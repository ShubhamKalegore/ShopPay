import {
  HttpErrorResponse,
  HttpInterceptorFn
} from '@angular/common/http';

import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (
  req,
  next
) => {

  const authService = inject(AuthService);

  if (req.url.includes('http://localhost:7000/api/chat')) {
    return next(req);
  }

  const clonedRequest = req.clone({
    withCredentials: true
  });

  return next(clonedRequest).pipe(
    catchError((error: HttpErrorResponse) => {

      if (
        error.status === 401 &&
        !req.url.includes('refresh-token')
      ) {

        return authService.refreshToken().pipe(
          switchMap(() => next(clonedRequest))
        );
      }

      return throwError(() => error);
    })
  );
};