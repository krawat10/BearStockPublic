import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, Subscription} from 'rxjs';
import {StockDashboard} from '../models/stock-dashboard';

@Injectable({providedIn: 'root'})
export class StockDashboardService {
  constructor(private http: HttpClient, @Inject('API_URL') private apiUrl: string) {
  }

  public fetchDashboards(): Observable<StockDashboard[]> {
    return this.http.get<StockDashboard[]>(this.apiUrl + `/Dashboards`);
  }

  public updateDashboard(dashboard: StockDashboard): Subscription {
    return this.http.put<StockDashboard>(this.apiUrl + `/Dashboards/` + dashboard.name, dashboard)
      .subscribe(x => console.log(x));
  }

  public addDashboard(dashboard: StockDashboard): Subscription {
    return this.http.post<StockDashboard>(this.apiUrl + `/Dashboards`, dashboard)
      .subscribe(x => console.log(x));
  }

  public removeDashboard(name: string): Subscription {
    return this.http.delete<StockDashboard>(this.apiUrl + `/Dashboards/` + name)
      .subscribe(x => console.log(x));
  }

}
