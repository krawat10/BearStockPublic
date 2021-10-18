import { CommonModule } from '@angular/common';
import { Component, NgModule } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { DxFormModule } from 'devextreme-angular/ui/form';
import { DxLoadIndicatorModule } from 'devextreme-angular/ui/load-indicator';

import { Observable } from 'rxjs';
import {AuthResponseData, AuthService } from '../../services';


@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent {
  loading = false;
  formData: any = {};
  error: string;

  constructor(private authService: AuthService, private router: Router) { }

  async onSubmit(e) {
    e.preventDefault();
    const { email, password, rememberMe } = this.formData;
    this.loading = true;
    let authObs: Observable<AuthResponseData>;

    authObs = await this.authService.signin(email, password, rememberMe);

    authObs.subscribe(
      resData => {
        this.loading = false;
        this.router.navigate(['/stocks']);
      },
      errorMessage => {
        this.error = errorMessage;
        this.loading = false;
      });
  }

  onCreateAccountClick = () => {
    this.router.navigate(['/create-account']);
  }
}
@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    DxFormModule,
    DxLoadIndicatorModule
  ],
  declarations: [ LoginFormComponent ],
  exports: [ LoginFormComponent ]
})
export class LoginFormModule { }
