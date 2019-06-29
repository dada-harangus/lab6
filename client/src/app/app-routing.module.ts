import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AuthGuard} from './guards/auth.guard';
import {RegisterComponent} from './components/register/register.component';
import {LoginComponent} from './components/login/login.component';
import {ExpensesComponent} from './components/expenses/expenses.component';
import {HomeComponent} from './components/home/home.component';
import {UsersComponent} from './components/users/users/users.component';
import {CommentsComponent} from './components/comments/comments.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'Expenses',
        component: ExpensesComponent,
      },
      //  {
      //   path: 'users',
      //   component: UsersComponent,
      //  }
    ]
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'users',
    component: UsersComponent
  },
  {
    path: 'comments',
    component: CommentsComponent
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
