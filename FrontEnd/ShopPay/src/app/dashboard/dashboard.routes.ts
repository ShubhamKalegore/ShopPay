import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';
import { ProductListComponent } from '../products/product-list/product-list.component';

export const DASHBOARD_ROUTES: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: '',
        component: DashboardHomeComponent
      },
      {
        path: 'products',
        component: ProductListComponent
      }
    ]
  }
];
