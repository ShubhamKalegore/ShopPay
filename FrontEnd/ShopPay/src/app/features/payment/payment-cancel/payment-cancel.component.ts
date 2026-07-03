import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment-cancel',
  imports: [],
  templateUrl: './payment-cancel.component.html',
  styleUrl: './payment-cancel.component.scss'
})
export class PaymentFailedComponent implements OnInit, OnDestroy {

  private timeoutId?: ReturnType<typeof setTimeout>;

  constructor(
    private readonly router: Router 
  ) { }

  ngOnInit(): void {

    this.timeoutId = setTimeout(() => {

      this.router.navigate(['/dashboard']);

    }, 10000);

  }

  ngOnDestroy(): void {

    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }

  }

}