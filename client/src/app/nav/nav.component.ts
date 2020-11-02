import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  // each form fields declared in the template will create a property using the 'name' defined for the field inside ds model object and on filling the form, the value will b d value filled in the browser
  model: any = {};

  //inject the AccountService and make it public so it can be accessed from the template
  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  //ngSubmit event will call ds method to submit the form
  login() {
    this.accountService.login(this.model).subscribe(
      (response) => {
        // we navigate the users to members area
        this.router.navigateByUrl('/members');
      },
      (error) => {
        console.log('Log in error ', error);
        // display the error msg from backend in a toast notification
        this.toastr.error(error.error);
      }
    );
  }

  logout() {
    this.accountService.logout();
    // we navigate user back to homepage on logout
    this.router.navigateByUrl('/');
  }
}
