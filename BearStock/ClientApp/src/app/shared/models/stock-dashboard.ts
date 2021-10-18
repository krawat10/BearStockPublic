import {Stock} from './stock';

export class StockDashboard {
  uuid: string;
  name: string;
  isDefault: boolean;
  stocks: Stock[];
}
