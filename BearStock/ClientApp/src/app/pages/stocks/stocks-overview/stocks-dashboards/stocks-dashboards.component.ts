import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {DxSelectBoxComponent} from 'devextreme-angular/ui/select-box';
import {DxTextBoxComponent} from 'devextreme-angular/ui/text-box';
import {DxValidationGroupComponent} from 'devextreme-angular/ui/validation-group';
import {Subscription} from 'rxjs';
import {DashboardStoreService} from '../../../../shared/services/dashboard-store.service';
import {StockStoreService} from '../../../../shared/services/stock-store.service';
import {StockDashboard} from '../../../../shared/models/stock-dashboard';

export enum State {
  Default = 0,
  NewDashboard = 1,
  NewDashboardDuplicate = 2
}

@Component({
  selector: 'app-stocks-dashboards',
  templateUrl: './stocks-dashboards.component.html',
  styleUrls: ['./stocks-dashboards.component.scss']
})
export class StocksDashboardsComponent implements OnInit, OnDestroy {
  @ViewChild(DxSelectBoxComponent, {static: false}) selectBox: DxSelectBoxComponent;
  @ViewChild('newDashboardValidation', {static: false}) newDashboardValidation: DxValidationGroupComponent;
  @ViewChild('dashboardInput', {static: false}) dashboardInput: DxTextBoxComponent;

  public stateType: typeof State = State;
  public state: State = this.stateType.Default;
  public backButtonOption: any = {icon: 'undo', type: 'default', onClick: () => this.state = State.Default};
  public addButtonOption: any = {icon: 'plus', type: 'default', onClick: () => this.addDashboard(this.dashboardInput)};
  private onChartsChange: Subscription;


  constructor(public dashboardStore: DashboardStoreService, private stockStore: StockStoreService) {
    this.isDuplicatedDashboardValidator = this.isDuplicatedDashboardValidator.bind(this);
  }

  private _selectedDashboard: StockDashboard;

  get selectedDashboard(): StockDashboard {
    return this._selectedDashboard;
  }

  set selectedDashboard(dashboard: StockDashboard) {
    if (this.onChartsChange) {
      this.onChartsChange?.unsubscribe();
    }
    this.stockStore.clearStocks();

    for (const stock of dashboard.stocks) {
      this.stockStore.addStock(stock);
    }

    this.onChartsChange = this.stockStore.stocks$.subscribe(value => {
      this._selectedDashboard = {
        ...dashboard,
        stocks: value
      };
      this.dashboardStore.updateDashboard(this._selectedDashboard);
    });
  }

  async setAsDefault(): Promise<void> {
    await this.dashboardStore.setDefaultDashboard(this.selectedDashboard.uuid);
  }

  async delete(): Promise<void> {
    await this.dashboardStore.removeDashboard(this.selectedDashboard.uuid);
  }

  public isDuplicatedDashboardValidator(params): boolean {
    return this.dashboardStore.getDashboardByName(params.value) === undefined;
  }


  async addDashboard(textBox: DxTextBoxComponent): Promise<void> {
    if (this.newDashboardValidation.instance.validate().isValid) {
      this.selectedDashboard = await this.dashboardStore.addDashboard(textBox.value);
      this.state = this.stateType.Default;
      textBox.writeValue('');
    }
  }

  changeDashboard($event: StockDashboard): void {
    if ($event !== this.selectedDashboard) {
      this.selectedDashboard = $event;
    }
  }

  async update(): Promise<void> {
    await this.dashboardStore.updateDashboard(this.selectedDashboard);
  }

  async addStock(value: string): Promise<void> {
    this.stockStore.addStock({
      uuid: null,
      ticket: value,
      chart: null,
      order: this.stockStore.count,
      stockPositions: []
    });
  }

  ngOnDestroy(): void {
    this.onChartsChange?.unsubscribe();
  }

  ngOnInit(): void {
    this.dashboardStore.dashboards$.subscribe(dashboards => {
      if (!this.selectedDashboard && dashboards.length > 0) {
        this.selectedDashboard = dashboards.find(item => item.isDefault) ?? dashboards[0];
      }
    });
  }
}
