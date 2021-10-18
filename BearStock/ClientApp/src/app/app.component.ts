import {Component, HostBinding, OnDestroy, OnInit} from '@angular/core';
import { AuthService, ScreenService, AppInfoService } from './shared/services';
import {Observable, Subscription} from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl:  './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  userSub: Subscription;
  isAuthenticated = false;
  @HostBinding('class') get getClass(): string {
    return Object.keys(this.screen.sizes).filter(cl => this.screen.sizes[cl]).join(' ');
  }

  constructor(private authService: AuthService, private screen: ScreenService, public appInfo: AppInfoService) { }

  ngOnInit(): void {
    this.authService.autoLogin();

    this.userSub = this.authService.user.subscribe(usr => this.isAuthenticated = !!usr);
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }
}
