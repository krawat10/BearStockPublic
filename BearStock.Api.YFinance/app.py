"""
This script runs the StockProvider application using a development server.
"""

from os import environ
from StockProvider import app

# az webapp up --sku F1 --name StockProvider

if __name__ == '__main__':
    HOST = environ.get('SERVER_HOST', 'localhost')
    try:
        PORT = int(environ.get('SERVER_PORT', '80'))
    except ValueError:
        PORT = 80
    app.run(HOST, PORT)
