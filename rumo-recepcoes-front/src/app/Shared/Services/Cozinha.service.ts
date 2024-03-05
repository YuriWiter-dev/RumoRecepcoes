import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Cozinha } from '../Models/Cozinha';
import { PageParams } from '../Models/page-params';
import { Page } from '../Models/page';
import { UtilService } from './Util.service';   


@Injectable({
  providedIn: 'root',
})
export class CozinhaService {

  urlBase = environment.BASE_URL;

  constructor(private _http: HttpClient) {}

  getCozinha() {
    return this._http.get<Cozinha[]>(
      `${this.urlBase}/cozinha`
    );
  }

  getCozinhasPage(pageParams: PageParams) {
    let params = UtilService.pageParams2HttpParams(pageParams);
    return this._http.get<Page<Cozinha>>(
      `${this.urlBase}/Cozinha`, {params: params}
    );
  }

  getCozinhaById(id: number) {
    return this._http.get<Cozinha>(
      `${this.urlBase}/Cozinha/${id}`
    );
  }

  salvarCozinha(id: number) {
    return this._http.post<Cozinha>(
      `${this.urlBase}/Cozinha`, id
    );
  }

  editarCozinha(Cozinha: Cozinha, id: number) {
    return this._http.put<Cozinha>(
      `${this.urlBase}/Cozinha/${id}`, Cozinha
    )
  }

  delete(id: number){
    return this._http.delete<Cozinha>(
      `${this.urlBase}/Cozinha/${id}`
    )
  }
}

