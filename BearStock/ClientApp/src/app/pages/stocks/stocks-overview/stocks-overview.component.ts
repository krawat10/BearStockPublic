import {Component, OnInit, ViewChild} from '@angular/core';
import {DxAccordionComponent} from 'devextreme-angular/ui/accordion';
import {ScreenService} from '../../../shared/services';


@Component({
  selector: 'app-stocks-overview',
  templateUrl: './stocks-overview.component.html',
  styleUrls: ['./stocks-overview.component.scss']
})
export class StocksOverviewComponent implements OnInit {
  @ViewChild(DxAccordionComponent, {static: false}) accordionComponent: DxAccordionComponent;

  public components: Array<{ title: string, name: string }> = [{
    title: 'Dashboards',
    name: 'stocks-dashboards'
  }, {
    title: 'Stocks Portfolio',
    name: 'stocks-portfolio'
  }, {
    title: 'Stocks Gain-Loss',
    name: 'stocks-gain-loss'
  }, {
    title: 'Stocks Positions',
    name: 'stocks-positions'
  }];

  constructor(private screen: ScreenService) {

  }

  ngOnInit(): void {
    this.screen.changed.subscribe(x => this.accordionComponent.instance.updateDimensions());
  }
}
