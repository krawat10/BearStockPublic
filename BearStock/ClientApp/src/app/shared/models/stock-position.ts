export interface StockPosition {
  id: string;
  ticket: string;
  pricePerShare: number;
  totalPrice: number;
  sharesAmount: number;
  date: Date;
}

export function clearStockPosition(): StockPosition {
  return {
    id: '',
    ticket: '',
    sharesAmount: 0,
    pricePerShare: 0.0,
    totalPrice: 0.0,
    date: new Date()
  };
}


