import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommmentsService {

  constructor(private  Http: HttpClient) {

  }

  getCommenst(filter?: any) {
    console.log(filter);

    return this.Http.get<any>('https://localhost:44336/api/comments?filter=' + filter
    ).toPromise();
  }

}
