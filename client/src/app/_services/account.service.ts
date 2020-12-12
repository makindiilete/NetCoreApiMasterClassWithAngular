import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';
import { environment } from '../../environments/environment';

/*An angular service is Singleton i.e. It stays on throughout the lifetime of the app until the app is closed or user leaves the app*/
//ds means ds service can be injected into other components or other services in our app
@Injectable({
  // with ds, we no longer need to explicitly import the service into the providers array of appmodule
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl; // replaced hardCoded api url with env variable
  private currentUserSource = new ReplaySubject<User>(1); //any component dt subscribes to ds User observable will get the current user stored in ds observable... here we setting d bufferSize to 1 which means we will only store single user
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient) {}

  login(model: any) {
    //our login api, body of the request (model which contains our username & password)
    return (
      this.http
        .post(this.baseUrl + 'account/login', model)
        // we add d pipe()
        .pipe(
          // we map the response returned from the observable checking if there is a response and if true, we store it in our localStorage
          map((response: User) => {
            const user = response;
            if (user) {
              this.setCurrentUser(user);
            }
          })
        )
    );
  }

  //register
  register(model: any) {
    return (
      this.http
        .post(this.baseUrl + 'account/register', model)
        // we add d pipe()
        .pipe(
          // we map the response returned from the observable checking if there is a response and if true, we store it in our localStorage
          map((response: User) => {
            const user = response;
            if (user) {
              this.setCurrentUser(user);
            }
            // we can optionally returned ds user to d subscriber but its not compulsory
            return user;
          })
        )
    );
  }

  //app.component.ts calls ds method sending the user retrieved from d localStorage as arg and we update d userSource
  setCurrentUser(user: User) {
    // d user is stored in d localStorage and the currentUserSource observable gts d stored user ready to return it to any subscribe via the 'currentUser$'
    localStorage.setItem('user', JSON.stringify(user));
    // we then set our observable source to the authenticated user
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    // we set our current user to null
    this.currentUserSource.next(null);
  }
}
