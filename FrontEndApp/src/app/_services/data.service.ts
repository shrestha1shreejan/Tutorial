import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/User';

// defining authoriaztion header to add to the api request
// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization : 'Bearer ' + localStorage.getItem('token')
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class DataService {
  baseUrl = environment.dataUrl;

  constructor(private httpClient: HttpClient) { }

  getUsers(): Observable<User[]> {
    // return this.httpClient.get<User[]>(this.baseUrl + 'data', httpOptions);
    return this.httpClient.get<User[]>(this.baseUrl + 'data');
  }

  getUser(id: string): Observable<User> {
    // return this.httpClient.get<User>(this.baseUrl + 'data/' + id, httpOptions);
    return this.httpClient.get<User>(this.baseUrl + 'data/' + id);
  }

  updateUser(id: string, user: User) {
    return this.httpClient.put(this.baseUrl + 'data/' + id, user);
  }

  setMainPhoto(userId: string, photoId: number) {
    return this.httpClient.post(this.baseUrl + 'data/' + userId + '/photos/' + photoId + '/setMain', {});
  }

  deletePhoto(userId: string, photoId: number) {
    return this.httpClient.delete(this.baseUrl + 'data/' + userId + '/photos/' + photoId);
  }

}
