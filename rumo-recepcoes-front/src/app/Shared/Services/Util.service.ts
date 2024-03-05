import { HttpParams } from "@angular/common/http";
import { PageParams } from "../Models/page-params";

export class UtilService {

  static pageParams2HttpParams(pageParams: PageParams): HttpParams {
    let params = new HttpParams();
    params = params.append('page', pageParams.page);
    params = params.append('linesPerPage', pageParams.linesPerPage);
    params = params.append('orderBy', pageParams.orderBy);
    params = params.append('direction', pageParams.direction);

    return params;
  }
}
