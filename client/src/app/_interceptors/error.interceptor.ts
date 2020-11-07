import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  // intercept() : - here we intercept either d request or response (next)
  // request : - d http request going out
  // next : - d http response coming back
  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    // ds returns an observable which we pipe() to peform further operation
    return next.handle(request).pipe(
      catchError((err) => {
        if (err) {
          switch (err.status) {
            case 400:
              // we av 2 types of 400 error in our app, d one we throw inside the buggy controller and the one inside register method of accountController that contains an errors object of all d validation errors
              if (err.error.errors) {
                const modelStateErrors = [];
                // we use d for...in too ilterate d object and pick d errors key
                for (let errorsKey in err.error.errors) {
                  if (err.error.errors.hasOwnProperty(errorsKey)) {
                    modelStateErrors.push(err.error.errors[errorsKey]);
                  }
                }
                console.log(modelStateErrors);
                throw modelStateErrors.reduce(
                  (previousValue, currentValue) =>
                    previousValue.concat(currentValue),
                  []
                );
              } else {
                // if its d normal 400 error from buggy controller
                this.toastr.error(err.statusText, err.status);
              }
              break;
            case 401:
              this.toastr.error(err.statusText, err.status);
              break;
            // for not found, we redirect them to a not-found page
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              // here we define extra object to pass along with our navigation
              const navigationExtras: NavigationExtras = {
                state: { error: err.error },
              };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(err);
          }
        }
        return throwError(err);
      })
    );
  }
}
