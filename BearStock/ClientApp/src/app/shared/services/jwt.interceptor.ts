import {Inject, Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthService} from './auth.service';
import {exhaustMap, take} from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, @Inject('API_URL') private apiUrl: string) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.user.pipe(
      take(1), // take once user variable
      exhaustMap(user => { // wait on user variable

        // do not add token if user doesn't exists (eg. login request)
        if (!user) {
          return next.handle(request);
        }

        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${user.token}`,
            'Access-Control-Allow-Headers': '*'
          },
        });

        // replace user observable with request observable
        return next.handle(request);
      })
    );
  }
}
