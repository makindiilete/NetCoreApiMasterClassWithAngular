import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  // ds field name will b called by the parent component to set the value received here to the value of the registerMode
  @Output() cancelRegister = new EventEmitter();

  registerForm: FormGroup;

  // minimum and maximum date allowed on the platform
  // minDate: Date;
  maxDate: Date;

  //ds contains array of all registration validation errors sent from the api
  validationErrors: string[] = [];

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private router: Router
  ) {
    // we ensure user cannot select any year dt is less than 18yrs from now
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: [
        null,
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)],
      ],
      confirmPassword: [
        '',
        [
          Validators.required,
          // we pass custom validation and d formField we want to match to
          this.matchValues('password'),
        ],
      ],
    });
  }

  //ds is our custom validator that confirms dt password and confirmPassword both match
  //we take a string parameter which is the 'formControlName' we want to match to i.e. 'password' & we return a validation function
  //ValidationFunction :- A function that receives a control and synchronously returns a map of validation errors if present, otherwise null.
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      // if password === confirmPassword ? return no error : return error of 'passwordMismatch' ('passwordMismatch' will b used in the template to render a validation msg)
      return control?.value === control?.parent?.controls[matchTo].value
        ? null
        : { passwordMismatch: true };
    };
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(
      (response) => {
        // on successful registration, we navigate the user to the member area
        this.router.navigateByUrl('/members');
      },
      (error) => {
        // if we have errors, we store it inside our validationErrors field
        this.validationErrors = error;
      }
    );
  }

  //
  cancel() {
    this.cancelRegister.emit(false);
  }

  // ds returns our username formControl so we can simply call 'userName' wherever we needs it
  get username() {
    return this.registerForm.get('username');
  }

  get password() {
    return this.registerForm.get('password');
  }
  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }
  get knownAs() {
    return this.registerForm.get('knownAs');
  }
  get dateOfBirth() {
    return this.registerForm.get('dateOfBirth');
  }
  get city() {
    return this.registerForm.get('city');
  }
  get country() {
    return this.registerForm.get('country');
  }
}
