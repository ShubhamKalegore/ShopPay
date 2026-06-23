import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CreateAddressPayload {
  userId: number;
  addressType: string;
  street: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  isDefault: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private readonly baseUrl = `${environment.apiUrl}/addresses`;

  constructor(private http: HttpClient) {}

  createAddress(payload: CreateAddressPayload): Observable<any> {
    return this.http.post(this.baseUrl, payload, { withCredentials: true });
  }
}
