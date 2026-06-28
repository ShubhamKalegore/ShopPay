import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../models/product';

export type CreateProductPayload = Omit<Product, 'productId'>;

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly baseUrl =
    `${environment.apiUrl}/products`;
    

  constructor(
    private http: HttpClient
  ) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(
      this.baseUrl,
      { withCredentials: true }
    );
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(
      `${this.baseUrl}/${id}`,
      { withCredentials: true }
    );
  }
  saveProduct(product: CreateProductPayload): Observable<Product> {
    return this.http.post<Product>(
      this.baseUrl,
      product,
      { withCredentials: true }
    );
  }
}
