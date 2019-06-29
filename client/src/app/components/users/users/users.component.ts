import { Component, OnInit } from '@angular/core';
import {Expense} from '../../../models/Expense';
import {PageEvent} from '@angular/material';
import {ExpensesService} from '../../../services/expenses.service';
import {User} from '../../../models/user';
import {UsersService} from '../../../services/users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  length: number;
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  public users: User [] = [];
  public displayedColumns: string[] = ['id', 'userRole', 'userName', ];

  pageEvent: PageEvent;
  activePageDataChunk = [];

  constructor(private usersService: UsersService) {
    // this.getAllExpenses(null, null, null);
    // this.activePageDataChunk = this.expenses.slice(0, this.pageSize);
    // console.log(this.activePageDataChunk);
    // console.log(this.expenses);
  }

  ngOnInit() {
    this.getAllUsers();


  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
  }

  onPageChanged(e) {
    const firstCut = e.pageIndex * e.pageSize;
    const secondCut = firstCut + e.pageSize;
    this.activePageDataChunk = this.users.slice(firstCut, secondCut);
  }
  getAllUsers() {
    // this.flowers = []
    this.usersService.getUsers().then(f => {
      this.users = f;
      this.length = this.users.length;
      this.activePageDataChunk = this.users.slice(0, this.pageSize);
      console.log(f);
      console.log(this.users);
      console.log(this.activePageDataChunk);
    });
  }
}
