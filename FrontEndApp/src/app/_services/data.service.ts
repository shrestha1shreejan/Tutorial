import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  baseUrl = 'https://localhost:44352/data/1c3e8cfb-5fb5-ec61-78f6-03541755b599';

  constructor(private httpClient: HttpClient) { }

  getData() {
    return this.httpClient.get(this.baseUrl);
  }

}
