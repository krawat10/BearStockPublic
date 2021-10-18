/* tslint:disable:variable-name */
export class User {
  constructor(
    public userId: string,
    public username: string,
    public email: string,
    private _token: string,
    private _tokenExpirationDate: Date,
    private _refreshToken: string,
    private _refreshTokenExpirationDate: Date) {
  }

  get token(): string {
    if (!this._tokenExpirationDate || new Date() > this._tokenExpirationDate) {
      return null;
    }

    return this._token;
  }

  get refreshToken(): string {
    if (!this._refreshTokenExpirationDate || new Date() > this._refreshTokenExpirationDate) {
      return null;
    }

    return this._refreshToken;
  }
}
