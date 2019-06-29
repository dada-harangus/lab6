import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private  Http: HttpClient) {
  }


  getUsers() {
    return this.Http.get<any>('https://localhost:44336/api/users'
    ).toPromise();
  }
}
