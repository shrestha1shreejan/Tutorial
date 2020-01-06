import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../_models/User';
import { PaginationResult } from '../_models/pagination';

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

  getUsers(page?, itemsPerPage?, userParams?): Observable<PaginationResult<User[]>> {
    // return this.httpClient.get<User[]>(this.baseUrl + 'data', httpOptions);

    const paginatedResult: PaginationResult<User[]> = new PaginationResult<User[]>();

    let params = new HttpParams();

    if (page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    return this.httpClient.get<User[]>(this.baseUrl + 'data', { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
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
