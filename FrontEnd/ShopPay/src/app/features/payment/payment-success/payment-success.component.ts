import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment-success',
  imports: [],
  templateUrl: './payment-success.component.html',
  styleUrl: './payment-success.component.scss'
})
export class PaymentSuccessComponent implements OnInit, OnDestroy {

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