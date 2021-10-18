from pandas import DataFrame


class Stock:
    def __init__(self, name: str, sector: str, category: str, industry: str, currency: str, pe: float, beta: float,
                 book_value: float, price_to_book: float, market: str, values: DataFrame):
        self.market = market
        self.price_to_book = price_to_book
        self.book_value = book_value
        self.beta = beta
        self.pe = pe
        self.currency = currency
        self.industry = industry
        self.category = category
        self.name = name
        self.sector = sector
        self.values = values

    def serialize(self):
        """Return object data in easily serializeable format"""
        return {
            'market': self.market,
            'price_to_book': self.price_to_book,
            'book_value': self.book_value,
            'beta': self.beta,
            'pe': self.pe,
            'currency': self.currency,
            'industry': self.industry,
            'category': self.category,
            'name': self.name,
            'sector': self.sector,
            'values': self.values.to_dict(orient='records')
        }
