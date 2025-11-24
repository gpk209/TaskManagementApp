import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AccountService } from '../services/accountService';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error) {
        switch (error.status) {
          case 401:
            // Only handle session expired for authenticated requests (not login/register)
            if (!req.url.includes('/login') && !req.url.includes('/register')) {
              console.log('Session expired - logging out...');
              // Logout user - this will trigger app.component.html to show home component
              accountService.logout();
              // Show alert and reload page after user dismisses it
              setTimeout(() => {
                alert('Session expired. Please login again.');
                window.location.reload();
              }, 100);
            }
            break;
          case 404:
            console.error('Resource not found');
            break;
          case 409:
            // Conflict error (e.g., username already exists)
            // Let the component handle this error
            console.log('Conflict error:', error.error);
            break;
          case 500:
            console.error('Internal server error');
            break;
          default:
            console.error('An error occurred:', error.message);
            break;
        }
      }
      return throwError(() => error);
    })
  );
};
