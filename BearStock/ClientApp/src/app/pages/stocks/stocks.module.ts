import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DxBoxModule} from 'devextreme-angular/ui/box';
import {DxScrollViewModule} from 'devextreme-angular/ui/scroll-view';
import {StocksChartsComponent} from './stocks-charts/stocks-charts.component';
import {StocksOverviewComponent} from './stocks-overview/stocks-overview.component';
import {StocksComponent} from './stocks.component';
import {RouterModule} from '@angular/router';
import {DxButtonModule} from 'devextreme-angular/ui/button';
import {DxiItemModule, DxiValidationRuleModule} from 'devextreme-angular/ui/nested';
import {DxFormModule} from 'devextreme-angular/ui/form';
import {DxPopoverModule} from 'devextreme-angular/ui/popover';
import {StocksChartPositionFormComponent} from './stocks-charts/stocks-chart/stocks-chart-position-form/stocks-chart-position-form.component';
import {StocksChartComponent} from './stocks-charts/stocks-chart/stocks-chart.component';
import {DxChartModule} from 'devextreme-angular/ui/chart';
import {DxButtonGroupModule} from 'devextreme-angular/ui/button-group';
import {DxRangeSelectorModule} from 'devextreme-angular/ui/range-selector';
import {DxLoadIndicatorModule} from 'devextreme-angular/ui/load-indicator';
import {DxSelectBoxModule} from 'devextreme-angular/ui/select-box';
import {DxValidatorModule} from 'devextreme-angular/ui/validator';
import {DxTextBoxModule} from 'devextreme-angular/ui/text-box';
import {DxAccordionModule} from 'devextreme-angular/ui/accordion';
import {DxValidationGroupModule} from 'devextreme-angular/ui/validation-group';
import {StocksDashboardsComponent} from './stocks-overview/stocks-dashboards/stocks-dashboards.component';
import {StocksGainLossComponent} from './stocks-overview/stocks-gain-loss/stocks-gain-loss.component';
import {DxPieChartModule} from 'devextreme-angular/ui/pie-chart';
import {StocksPortfolioComponent} from './stocks-overview/stocks-portfolio/stocks-portfolio.component';
import {DxDataGridModule} from 'devextreme-angular/ui/data-grid';
import {StocksPositionsComponent} from './stocks-overview/stocks-positions/stocks-positions.component';
import {AngularResizedEventModule} from 'angular-resize-event';

@NgModule({
  imports: [
    CommonModule,
    AngularResizedEventModule,
    DxiItemModule,
    DxiValidationRuleModule,
    DxFormModule,
    DxPopoverModule,
    DxButtonGroupModule,
    DxRangeSelectorModule,
    DxBoxModule,
    DxScrollViewModule,
    DxChartModule,
    DxPieChartModule,
    DxLoadIndicatorModule,
    DxDataGridModule,
    DxSelectBoxModule,
    DxButtonModule,
    DxValidatorModule,
    DxTextBoxModule,
    DxAccordionModule,
    DxValidationGroupModule,
    RouterModule.forChild([{path: '', component: StocksComponent}])
  ],
  declarations: [
    StocksComponent,
    StocksChartPositionFormComponent,
    StocksChartComponent,
    StocksChartsComponent,
    StocksDashboardsComponent,
    StocksGainLossComponent,
    StocksPortfolioComponent,
    StocksPositionsComponent,
    StocksOverviewComponent
  ],
  exports: [StocksComponent]
})
export class StocksModule {
}
