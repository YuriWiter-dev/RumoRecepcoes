import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Copa } from '../Models/Copa';
import { PageParams } from '../Models/page-params';
import { Page } from '../Models/page';
import { UtilService } from './Util.service';   


@Injectable({
  providedIn: 'root',
})
export class CopaService {

  urlBase = environment.BASE_URL;

  constructor(private _http: HttpClient) {}

  getCopa() {
    return this._http.get<Copa[]>(
      `${this.urlBase}/Copa`
    );
  }

  getCopasPage(pageParams: PageParams) {
    let params = UtilService.pageParams2HttpParams(pageParams);
    return this._http.get<Page<Copa>>(
      `${this.urlBase}/Copa`, {params: params}
    );
  }

  getCopaById(id: number) {
    return this._http.get<Copa>(
      `${this.urlBase}/Copa/${id}`
    );
  }

  salvarCopa(id: number) {
    return this._http.post<Copa>(
      `${this.urlBase}/Copa`, id
    );
  }

  editarCopa(Copa: Copa, id: number) {
    return this._http.put<Copa>(
      `${this.urlBase}/Copa/${id}`, Copa
    )
  }

  delete(id: number){
    return this._http.delete<Copa>(
      `${this.urlBase}/Copa/${id}`
    )
  }
}

