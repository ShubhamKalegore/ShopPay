import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly baseUrl = 'https://localhost:9001/api/users';

  constructor(private http: HttpClient) {}

  login(payload: any): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/login`,
      payload,
      { withCredentials: true }
    );
  }

  refreshToken(): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/refresh-token`,
      {},
      { withCredentials: true }
    );
  }

  logout(): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/logout`,
      {},
      { withCredentials: true }
    );
  }

  isLoggedIn(): boolean {
    return true;
  }
}