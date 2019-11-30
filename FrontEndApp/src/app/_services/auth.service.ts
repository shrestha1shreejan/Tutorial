import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseurl = 'https://localhost:44352/auth/';

  constructor(private httpClient: HttpClient) { }

  login(model: any) {
    return this.httpClient.post(this.baseurl + 'login', model)
    .pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
  }

  register(model: any) {
    return this.httpClient.post(this.baseurl + 'register', model);
  }
 
}
