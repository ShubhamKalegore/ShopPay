import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ChatRequest } from '../models/chat-request';
import { ChatResponse } from '../models/chat-response';

@Injectable({
  providedIn: 'root'
})
export class AiService {

  private readonly apiUrl = 'http://localhost:7000/api/chat';

  constructor(private http: HttpClient) { }

  sendMessage(message: string): Observable<ChatResponse> {

    const request: ChatRequest = {
      message
    };

    return this.http.post<ChatResponse>(
      this.apiUrl,
      request
    );
  }
}