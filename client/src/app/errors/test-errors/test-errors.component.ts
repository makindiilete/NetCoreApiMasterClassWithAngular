import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css'],
})
export class TestErrorsComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api';
  validationErrors: string[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  //testing for 400 error
  get400() {
    this.http.get(this.baseUrl + '/buggy/bad-request').subscribe(
      (response) => console.log(response),
      (error) => console.log(error)
    );
  }

  get400ValidationError() {
    // sending an empty object to the registration endpoint of the account controller to test 400 validation error
    this.http.post(this.baseUrl + '/account/register', {}).subscribe(
      (response) => console.log(response),
      (error) => {
        console.log(error);
        this.validationErrors = error;
      }
    );
  }

  //testing for 401 error
  get401() {
    this.http.get(this.baseUrl + '/buggy/auth').subscribe(
      (response) => console.log(response),
      (error) => console.log(error)
    );
  }

  //testing for 404 error
  get404() {
    this.http.get(this.baseUrl + '/buggy/not-found').subscribe(
      (response) => console.log(response),
      (error) => console.log(error)
    );
  }

  //testing for 500 error
  get500() {
    this.http.get(this.baseUrl + '/buggy/server-error').subscribe(
      (response) => console.log(response),
      (error) => console.log(error)
    );
  }
}
