import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { DataService } from '../_services/data.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable()
export class MemberListResolver implements Resolve<User[]> {

    pageNumber = 1;
    pageSize = 4;

    constructor(private dataService: DataService, private alertify: AlertifyService, private router: Router) { }

    resolve(): Observable<User[]> {
        return this.dataService.getUsers(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
