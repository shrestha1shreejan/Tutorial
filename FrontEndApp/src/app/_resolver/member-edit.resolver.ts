import { AuthService } from './../_services/auth.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { DataService } from '../_services/data.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable()
export class MemberEditResolver implements Resolve<User> {

    constructor(private dataService: DataService, private alertify: AlertifyService, private router: Router,
                private authService: AuthService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.dataService.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error('Problem getting user data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
