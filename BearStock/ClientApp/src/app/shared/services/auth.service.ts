import {Inject, Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {User} from '../models/user';
import {catchError, tap} from 'rxjs/operators';
import {BehaviorSubject, Observable, throwError} from 'rxjs';

export interface AuthResponseData {
  id: string;
  email: string;
  userId: string;
  username: string;
  accessToken: string;
  refreshToken: string;
  scope: string;
  expiresIn: number;
  refreshExpiresIn: number;
}

@Injectable()
export class AuthService {
  user = new BehaviorSubject<User>(null);
  private tokenExpirationTimer = null;

  constructor(private router: Router, private http: HttpClient, @Inject('AUTH_URL') private authUrl: string) {
  }

  signup(username: string, email: string, password: string): Observable<AuthResponseData> {
    return this.http.post<AuthResponseData>(
      `${this.authUrl}/Users/Register`,
      {
        username,
        email,
        password
      }
    ).pipe(catchError(this.handleError));
  }

  signin(email: string, password: string, rememberMe: boolean): Observable<AuthResponseData> {
    return this.http.post<AuthResponseData>(
      `${this.authUrl}/Users/Authenticate`,
      {
        usernameOrEmail: email,
        includeRefreshToken: rememberMe,
        password
      }
    ).pipe(catchError(this.handleError), tap((resData: AuthResponseData) =>
      this.handleAuthentication(resData.userId, resData.username, resData.email, resData.accessToken, resData.expiresIn, resData.refreshToken, resData.refreshExpiresIn)));
  }

  autoLogin(): void {
    const userData: {
      userId: string;
      username: string;
      email: string;
      id: string;
      _token: string;
      _tokenExpirationDate: string;
      _refreshToken: string;
      _refreshTokenExpirationDate: string;
    } = JSON.parse(localStorage.getItem('userData'));

    if (!userData) {
      return;
    }

    const loadedUser = new User(
      userData.userId,
      userData.username,
      userData.email,
      userData._token,
      new Date(userData._tokenExpirationDate),
      userData._refreshToken,
      new Date(userData._refreshTokenExpirationDate)
    );

    if (loadedUser.token) {
      this.user.next(loadedUser);

      // Setup auto logout
      const expirationDuration = new Date(userData._tokenExpirationDate).getTime() - new Date().getTime();
      this.autoLogout(expirationDuration);
    }
  }

  autoLogout(expirationDuration: number): void {
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationDuration);
  }

  logout(): void {
    this.user.next(null);
    this.router.navigate(['/auth']);
    localStorage.removeItem('userData');

    if (this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }

    this.tokenExpirationTimer = null;
  }

  private handleAuthentication(
    userId: string,
    username: string,
    email: string,
    accessToken: string,
    expiresIn: number,
    refreshToken: string,
    refreshExpiresIn: number): void {
    // parse to date
    const expirationDate = new Date(new Date().getTime() + expiresIn * 1000);
    const refreshExpirationDate = new Date(new Date().getTime() + refreshExpiresIn * 1000);

    const user = new User(
      userId,
      username,
      email,
      accessToken,
      expirationDate,
      refreshToken,
      refreshExpirationDate
    );

    this.user.next(user);
    this.autoLogout(expiresIn * 1000);

    localStorage.setItem('userData', JSON.stringify(user));
  }

  private handleError(errorRes: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred.';

    switch (errorRes.error.error.message) {
      case 'EMAIL_EXISTS':
        errorMessage = 'Email already exists.';
        break;
      case 'EMAIL_NOT_FOUND':
        errorMessage = 'Email does not exists.';
        break;
      case 'INVALID_PASSWORD':
        errorMessage = 'Invalid password.';
        break;
    }

    return throwError(errorMessage);
  }
}

