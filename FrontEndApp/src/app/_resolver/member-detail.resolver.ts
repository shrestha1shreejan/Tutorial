import { catchError } from 'rxjs/operators';
import { AlertifyService } from './../_services/alertify.service';
import { DataService } from './../_services/data.service';
import { Observable, of } from 'rxjs';
import { User } from './../_models/User';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable()
export class MemberDetailResolver implements Resolve<User> {

    constructor(private dataService: DataService, private alertify: AlertifyService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.dataService.getUser(route.params.id).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
