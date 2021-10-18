import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import * as uuidGenerator from 'uuid';
import {StockDashboardService} from './dashboard.service';
import {StockDashboard} from '../models/stock-dashboard';

@Injectable({providedIn: 'root'})
export class DashboardStoreService {
  constructor(private stockDashboardService: StockDashboardService) {
    stockDashboardService
      .fetchDashboards()
      .subscribe(dashboards => {
        this.dashboards = dashboards.map(x => {
          return {uuid: uuidGenerator.v4(), ...x};
        });

        console.log(this.dashboards);
      });
  }

  private _dashboards = new BehaviorSubject<StockDashboard[]>([]);

  readonly dashboards$: Observable<StockDashboard[]> = this._dashboards.asObservable();

  private get dashboards(): StockDashboard[] {
    return this._dashboards.getValue();
  }

  private set dashboards(val: StockDashboard[]) {
    this._dashboards.next(val);
  }

  getDashboard(uuid: string): StockDashboard {
    return this._dashboards.getValue().find(x => x.uuid === uuid);
  }

  getDashboardByName(name: string): StockDashboard {
    return this._dashboards.getValue().find(x => x.name === name);
  }


  async addDashboard(name: string): Promise<StockDashboard> {
    const dashboard: StockDashboard = {uuid: uuidGenerator.v4(), name, stocks: [], isDefault: false};

    this.stockDashboardService.addDashboard(dashboard);

    this.dashboards = [...this.dashboards, dashboard];

    return dashboard;
  }

  async removeDashboard(uuid: string): Promise<void> {
    const dashboard = this.dashboards.find(stock => stock.uuid === uuid);

    try {
      await this.stockDashboardService.removeDashboard(dashboard.name);
    } catch (e) {
      console.error(e);
    } finally {
      this.dashboards = [...this.dashboards.filter(d => d !== dashboard)];
    }
  }

  async updateDashboard(dashboard: StockDashboard): Promise<void> {
    try {
      await this.stockDashboardService.updateDashboard(dashboard);
    } catch (e) {
      console.error(e);
    }

    this.dashboards = this.dashboards.map(old => {
      return old.name === dashboard.name ? dashboard : old;
    });
  }

  async setDefaultDashboard(uuid: string = null): Promise<void> {
    for (const dashboard of this.dashboards) {
      if (uuid == null) {
        dashboard.isDefault = true;
        this.stockDashboardService.updateDashboard(dashboard);
      } else if (dashboard.uuid === uuid) {
        dashboard.isDefault = true;
        this.stockDashboardService.updateDashboard(dashboard);
      } else if (dashboard.isDefault) {
        dashboard.isDefault = false;
        this.stockDashboardService.updateDashboard(dashboard);
      }
    }

    this._dashboards.next(this.dashboards);
  }
}
