import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  canActivate(): Observable<boolean> {
    // we get the currentUser observable and map the returned response
    return this.accountService.currentUser$.pipe(
      // from the response we check if we have a user then we return a true as a boolean observable
      map((user) => {
        if (user) {
          return true;
        } else {
          this.toastr.error('You shall not pass!');
        }
      })
    );
  }
}
