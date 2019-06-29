import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Expense} from '../models/Expense';

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {

  constructor(private  Http: HttpClient) {
  }

  getExpenses(type?: any, DateFrom?: any, DateTo?: any) {

    console.log(DateTo);
    console.log(DateFrom);
    let from;
    let to;
    if (DateFrom != null && DateTo != null) {
      const day = DateFrom.getDate();       // yields date
      const month = DateFrom.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
      const year = DateFrom.getFullYear();  // yields year
      const hour = DateFrom.getHours();     // yields hours
      const minute = DateFrom.getMinutes(); // yields minutes
      const second = DateFrom.getSeconds(); // yields seconds

// After this construct a string with the above results as below
      from = day + '/' + 0 + month + '/' + year + ' ' + 0 + hour + ':' + 0 + minute + ':' + 0 + second;

      const day1 = DateTo.getDate();       // yields date
      const month1 = DateTo.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
      const year1 = DateTo.getFullYear();  // yields year
      const hour1 = DateTo.getHours();     // yields hours
      const minute1 = DateTo.getMinutes(); // yields minutes
      const second1 = DateTo.getSeconds(); // yields seconds

// After this construct a string with the above results as below
      to = day1 + '/' + 0 + month1 + '/' + year1 + ' ' + 0 + hour1 + ':' + 0 + minute1 + ':' + 0 + second1;
      console.log(from);
      console.log(to);
    }
    if (type == null) {
      type = 'none';
    }
    return this.Http.get<any>(`https://localhost:44336/api/Expenses?from=` + from +
      '&to=' + to + '&type=' + type + '&page=2'
    ).toPromise();

  }
}
