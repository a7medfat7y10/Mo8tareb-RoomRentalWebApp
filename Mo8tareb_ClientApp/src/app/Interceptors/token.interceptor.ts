import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AccountApiService } from '../Services/account-api.service';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private AuthCrud : AccountApiService,private route:Router) {}
  token: any = '';
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    this.token = this.AuthCrud.GetToken();
    if (this.token) {
      request = request.clone({
        setHeaders:{Authorization:`Bearer ${this.token}`}
      })
    }
    return next.handle(request).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status == 401) {
            this.route.navigate(["/login/"]);
          }
          if (err.status == 403)
          this.route.navigate(["/unAuthorized/"]);

        }
        return throwError(()=>new Error("Error occured :("));
      })
    );
  }
}
