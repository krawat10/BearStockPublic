import typing
import yfinance as yf
import html5lib
import bs4
from pandas_datareader import data as pdr

yf.pdr_override()

from StockProvider.models.stock import Stock


class StockServices:
    def get_stock_history(self, ticket: str, interval: str, period='3y'):
        stock = yf.Ticker(ticket)
        stock_info = stock.info

        df = pdr.get_data_yahoo(ticket, period=period, interval=interval)
        df['d'] = df.index
        df.rename(columns={"Open": "o"}, inplace=True)
        df.rename(columns={"Close": "c"}, inplace=True)
        df.rename(columns={"High": "h"}, inplace=True)
        df.rename(columns={"Low": "l"}, inplace=True)
        df['o'] = df['o'].apply(lambda x: round(x, 2))
        df['c'] = df['c'].apply(lambda x: round(x, 2))
        df['h'] = df['h'].apply(lambda x: round(x, 2))
        df['l'] = df['l'].apply(lambda x: round(x, 2))

        del df['Adj Close']
        del df['Volume']

        sector = stock_info.get('sector', None)
        category = stock_info.get('category', None)
        industry = stock_info.get('industry', None)
        currency = stock_info.get('currency', None)
        market = stock_info.get('market', None)
        pe = stock_info.get('trailingPE', 0.0)
        beta = stock_info.get('beta', 0.0)
        book_value = stock_info.get('bookValue', 0.0)
        price_to_book = stock_info.get('priceToBook', 0.0)

        return Stock(ticket, sector, category, industry, currency, pe, beta, book_value, price_to_book, market, df)
