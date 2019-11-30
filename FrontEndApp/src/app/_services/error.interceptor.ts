import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(err => {
                /// handle all 401 type error
                if (err.status === 401) {
                    return throwError(err.statusText);
                }
                if (err instanceof HttpErrorResponse) {
                    /// handle all 500 type error
                    const applicationError = err.headers.get('Application-Error');
                    if (applicationError) {
                        return throwError(applicationError);
                    }

                    /// handle all modelStateError
                    const serverError = err.error;
                    let modelStateError = '';
                    if (serverError.errors && typeof serverError.errors === 'object') {
                        for (const key in serverError.errors) {
                            if (serverError.errors[key]) {
                                // append all model state error
                                modelStateError += serverError.errors[key] + '\n';
                            }
                        }
                    }

                    return throwError(modelStateError || serverError || 'Server Error');
                }
            })
        );
    }

}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
}