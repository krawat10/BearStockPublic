"""
Routes and views for the flask application.
"""
import urllib.parse
from datetime import datetime

import flask
from flask import render_template, abort, jsonify, request
from flask_cors import cross_origin

from StockProvider import app, cache
from StockProvider.services.stock_services import StockServices


def cache_key():
    args = flask.request.args
    key = flask.request.path + '?' + urllib.parse.urlencode([
        (k, v) for k in sorted(args) for v in sorted(args.getlist(k))
    ])
    return key


@app.route('/')
@app.route('/home')
def home():
    """Renders the home page."""
    return render_template(
        'index.html',
        title='Home Page',
        year=datetime.now().year,
    )


@app.route('/api/stocks/<ticket>')
@cross_origin()
@cache.cached(timeout=3600, key_prefix=cache_key)
def get(ticket):
    """
    This function responds to a request for /api/stocks/{ticket}
    with one matching person from people

    :param ticket:   ticket of the stock
    :return:        stock data
    """
    try:
        interval = request.args.get('interval', '1d')
        period = request.args.get('period', '3y')
        stock = StockServices().get_stock_history(ticket, interval, period)
    except ValueError as ex:
        abort(404, "Stock with ticket {ticket} not found. {ex}".format(ticket=ticket, ex=ex))
    else:
        return jsonify(stock.serialize())
