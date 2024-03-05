import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Garcom } from '../Models/Garcom';
import { PageParams } from '../Models/page-params';
import { Page } from '../Models/page';
import { UtilService } from './Util.service';   
import { environment } from '../../environments/environment';



@Injectable({
  providedIn: 'root',
})
export class GarcomService {

  urlBase = environment.BASE_URL;

  constructor(private _http: HttpClient) {}

  getPedidos() {
    return this._http.get<Garcom[]>(
      `${this.urlBase}/pedidos`
    );//https://localhost:5001/pedidos'
  }

  getGarcomsPage(pageParams: PageParams) {
    let params = UtilService.pageParams2HttpParams(pageParams);
    return this._http.get<Page<Garcom>>(
      `${this.urlBase}/Garcom`, {params: params}
    );
  }

  getGarcomById(id: number) {
    return this._http.get<Garcom>(
      `${this.urlBase}/Garcom/${id}`
    );
  }

  salvarPedido(pedido: any) {
    return this._http.post<Garcom>(
      `${this.urlBase}/pedido`, pedido
    );
  }

  editarGarcom(Garcom: Garcom, id: number) {
    return this._http.put<Garcom>(
      `${this.urlBase}/Garcom/${id}`, Garcom
    )
  }

  delete(id: number){
    return this._http.delete<Garcom>(
      `${this.urlBase}/Garcom/${id}`
    )
  }
}

