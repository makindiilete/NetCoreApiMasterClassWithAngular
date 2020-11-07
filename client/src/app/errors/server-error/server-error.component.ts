import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css'],
})
export class ServerErrorComponent implements OnInit {
  error: any;
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    // we gt the error object passed to the navigation state
    // ds state is only available once d page is loaded, refreshing the page removes d state
    this.error = navigation?.extras?.state?.error;
  }

  ngOnInit(): void {}
}
