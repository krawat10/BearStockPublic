"""
The flask application package.
"""

from flask import Flask
from flask_cors import CORS
from flask_caching import Cache

app = Flask(__name__)
cache = Cache(app, config={'CACHE_TYPE': 'simple'})
CORS(app)

import StockProvider.views
