import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {DxFormComponent} from 'devextreme-angular/ui/form';
import {dxFormSimpleItem} from 'devextreme/ui/form';
import {Stock} from 'src/app/shared/models/stock';
import {clearStockPosition, StockPosition} from '../../../../../shared/models/stock-position';
import {StockStoreService} from '../../../../../shared/services/stock-store.service';

@Component({
  selector: 'app-stock-charts-item-history-form',
  templateUrl: './stocks-chart-position-form.component.html',
  styleUrls: ['./stocks-chart-position-form.component.scss']
})
export class StocksChartPositionFormComponent implements OnInit {
  @Input() public target: string;
  @Input() public visible: boolean;
  @Input() public stock: Stock;
  @Output() visibleChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @ViewChild(DxFormComponent) form: DxFormComponent;

  public stockHistory: StockPosition = clearStockPosition();

  constructor(public stockStoreService: StockStoreService) {
  }

  public get isVisible(): boolean {
    return this.visible;
  }

  public set isVisible(value: boolean) {
    this.visible = value;
    this.visibleChange.emit(value);
  }

  ngOnInit(): void {
  }

  onFormChange($event: dxFormSimpleItem): void {
    const {sharesAmount, pricePerShare, totalPrice} = this.stockHistory;
    switch ($event.dataField) {
      case 'pricePerShare': {
        if (sharesAmount > 0) {
          this.stockHistory.totalPrice = this.stockHistory.pricePerShare * this.stockHistory.sharesAmount;
        }
        break;
      }
      case 'sharesAmount': {
        if (pricePerShare > 0) {
          this.stockHistory.totalPrice = this.stockHistory.pricePerShare * this.stockHistory.sharesAmount;
        }
        break;
      }
      case 'totalPrice': {
        if (pricePerShare > 0) {
          this.stockHistory.sharesAmount = this.stockHistory.totalPrice / this.stockHistory.pricePerShare;
        }
        break;
      }
      case 'date': {
        if (pricePerShare > 0) {
          this.stockHistory.date = new Date(this.stockHistory.date);
        }
        break;
      }
    }
  }

  async save(): Promise<void> {
    this.stock.stockPositions.push(this.stockHistory);
    await this.stockStoreService.updateStock(this.stock);

    this.stockHistory = clearStockPosition();
    this.isVisible = false;
  }
}
