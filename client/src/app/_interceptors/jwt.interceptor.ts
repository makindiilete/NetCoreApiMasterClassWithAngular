import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  // we inject our accountService where we have the current user observable which contains the token
  constructor(private accountService: AccountService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    let currentUser: User; // ds will contain the content of the current user dt we store in localStorage

    // we using the 'take(1)' here so dt we will not need to unsubscribe from ds observable.. once we have taken what we need, d subscription will complete..
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (currentUser = user));
    // we attach the token for every request when we are logged in
    if (currentUser) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`,
        },
      });
    }
    return next.handle(request);
  }
}
