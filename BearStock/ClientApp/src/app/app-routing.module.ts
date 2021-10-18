import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginFormComponent, ResetPasswordFormComponent, CreateAccountFormComponent, ChangePasswordFormComponent } from './shared/components';
import { AuthGuard } from './shared/services/auth.guard';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { DxDataGridModule } from 'devextreme-angular/ui/data-grid';
import {  DxFormModule } from 'devextreme-angular/ui/form';

const routes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [ AuthGuard ]
  },
  {
    path: 'stocks',
    loadChildren: () => import('./pages/stocks/stocks.module').then(x => x.StocksModule),
    canActivate: [ AuthGuard ]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [ AuthGuard ]
  },
  {
    path: 'login-form',
    component: LoginFormComponent
  },
  {
    path: 'reset-password',
    component: ResetPasswordFormComponent,
    canActivate: [ AuthGuard ]
  },
  {
    path: 'create-account',
    component: CreateAccountFormComponent
  },
  {
    path: 'change-password/:recoveryCode',
    component: ChangePasswordFormComponent,
    canActivate: [ AuthGuard ]
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes), DxFormModule, DxDataGridModule],
  providers: [AuthGuard],
  exports: [RouterModule],
  declarations: [HomeComponent, ProfileComponent]
})
export class AppRoutingModule { }
