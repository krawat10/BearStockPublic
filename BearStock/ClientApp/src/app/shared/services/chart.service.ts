import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Chart} from '../models/chart';
import {map, tap} from 'rxjs/operators';
import * as uuid_generator from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class ChartService {
  constructor(private http: HttpClient, @Inject('API_URL') private apiUrl: string) {
  }

  public fetchChart(ticket: string, interval = '1d', period = '100d'): Promise<Chart> {
    return this.http
      .get<Chart>(this.apiUrl + `/stocks/${ticket}?interval=${interval}&period=${period}`)
      .pipe(map(ChartSerializer.serialize))
      .toPromise();
  }
}

class ChartSerializer {
  static serialize({sector, name, bookValue, currency, pe, values}): Chart {
    return {uuid: uuid_generator.v4(), sector, name, bookValue, currency, pe, values: values.map(x => ({...x, d: new Date(x.d)}))};
  }
}
