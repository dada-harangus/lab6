import {Component, OnInit} from '@angular/core';
import {User} from '../../models/user';
import {PageEvent} from '@angular/material';
import {UsersService} from '../../services/users.service';
import {CommmentsService} from '../../services/commments.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  filter: any;
  length: number;
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  public comments: Comment [] = [];
  public displayedColumns: string[] = ['id', 'Text', 'Important', 'ExpenseId'];

  pageEvent: PageEvent;
  activePageDataChunk = [];

  constructor(private commentService: CommmentsService) {
    // this.getAllExpenses(null, null, null);
    // this.activePageDataChunk = this.expenses.slice(0, this.pageSize);
    // console.log(this.activePageDataChunk);
    // console.log(this.expenses);
  }

  ngOnInit() {
    this.getAllComments();


  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
  }

  onPageChanged(e) {
    const firstCut = e.pageIndex * e.pageSize;
    const secondCut = firstCut + e.pageSize;
    this.activePageDataChunk = this.comments.slice(firstCut, secondCut);
  }

  getAllComments(filter?: any) {
    // this.flowers = []
    this.commentService.getCommenst(filter).then(f => {
      this.comments = f;
      this.length = this.comments.length;
      this.activePageDataChunk = this.comments.slice(0, this.pageSize);
      console.log(f);
      console.log(this.comments);
      console.log(this.activePageDataChunk);
    });
  }

  FilterType() {
    this.getAllComments(this.filter);
    console.log(this.filter);
  }
}
