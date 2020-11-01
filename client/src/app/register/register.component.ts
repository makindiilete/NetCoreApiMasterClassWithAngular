import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  model: any = {};

  // ds field name will b called by the parent component to set the value received here to the value of the registerMode
  @Output() cancelRegister = new EventEmitter();
  constructor(private accountService: AccountService) {}

  ngOnInit(): void {}

  register() {
    this.accountService.register(this.model).subscribe(
      (response) => {
        console.log(response);
        this.cancel();
      },
      (error) => console.log(error)
    );
  }

  //
  cancel() {
    this.cancelRegister.emit(false);
  }
}
