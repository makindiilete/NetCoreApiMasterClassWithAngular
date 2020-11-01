import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  // each form fields declared in the template will create a property using the 'name' defined for the field inside ds model object and on filling the form, the value will b d value filled in the browser
  model: any = {};

  //inject the AccountService and make it public so it can be accessed from the template
  constructor(public accountService: AccountService) {}

  ngOnInit(): void {}

  //ngSubmit event will call ds method to submit the form
  login() {
    this.accountService.login(this.model).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => console.log('Log in error ', error)
    );
  }

  logout() {
    this.accountService.logout();
  }
}
