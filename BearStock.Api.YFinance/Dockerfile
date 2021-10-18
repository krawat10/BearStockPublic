#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM python:3.7.2-alpine3.9
WORKDIR /code

ENV FLASK_APP=app.py
ENV FLASK_RUN_HOST=0.0.0.0
ENV FLASK_RUN_PORT=80

RUN apk add --no-cache libxml2-dev libxslt-dev python3-dev libstdc++ && \
    apk add --no-cache g++ && \
    ln -s /usr/include/locale.h /usr/include/xlocale.h && \
    pip3 install numpy && \
    pip3 install pandas

COPY ["BearStock.Api.YFinance/requirements.txt", "requirements.txt"]
RUN pip install -r requirements.txt
EXPOSE 80
COPY BearStock.Api.YFinance/ .
CMD ["flask", "run"]