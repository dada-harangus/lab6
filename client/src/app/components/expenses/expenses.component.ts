import {Component, OnInit} from '@angular/core';
import {ExpensesService} from '../../services/expenses.service';
import {MatDatepickerInputEvent} from '@angular/material/datepicker';
import {MatDatepicker} from '@angular/material/datepicker';
import {Expense} from '../../models/Expense';
import {PageEvent} from '@angular/material';

@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.scss']
})
export class ExpensesComponent implements OnInit {
  length: number;
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  type: any;
  public expenses: Expense [] = [];
  public displayedColumns: string[] = ['Description', 'Sum', 'Location', 'Date', 'Currency', 'ExpenseType', 'NumberOfComments'];
  public selectedStartDate: Date;
  public selectedEndDate: Date;
  pageEvent: PageEvent;
  activePageDataChunk = [];

  constructor(private expensesService: ExpensesService) {
    // this.getAllExpenses(null, null, null);
    // this.activePageDataChunk = this.expenses.slice(0, this.pageSize);
    // console.log(this.activePageDataChunk);
    // console.log(this.expenses);
  }

  ngOnInit() {
    this.getAllExpenses(null, null, null);


  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
  }

  onPageChanged(e) {
    const firstCut = e.pageIndex * e.pageSize;
    const secondCut = firstCut + e.pageSize;
    this.activePageDataChunk = this.expenses.slice(firstCut, secondCut);
  }

  addEventStartDate(event: MatDatepickerInputEvent<Date>) {
    this.selectedStartDate = event.value;
  }

  addEventEndDate(event: MatDatepickerInputEvent<Date>) {
    this.selectedEndDate = event.value;
  }

  getAllExpenses(ExpenseType?: any, DateFrom?: Date, DateTo?: Date) {
    // this.flowers = []
    this.expensesService.getExpenses(ExpenseType, DateFrom, DateTo).then(f => {
      this.expenses = f;
      this.length = this.expenses.length;
      this.activePageDataChunk = this.expenses.slice(0, this.pageSize);
      console.log(f);
      console.log(this.expenses);
      console.log(this.activePageDataChunk);
    });
  }

  submit() {
    this.getAllExpenses(null, this.selectedStartDate, this.selectedEndDate);
  }

  FilterType() {
    this.getAllExpenses(this.type, null, null);
    console.log(this.type);
  }

  addType(Event: string) {
    this.type = Event;
    console.log(Event);
  }
}
