import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/user/login/login.component';
import { DashboardComponent } from './components/page/dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { RegisterComponent } from './components/user/register/register.component';
import { TaskDetailsComponent } from './components/page/task-details/task-details.component';
import { TasklistComponent } from './components/page/tasklist/tasklist.component';
import { UserTaskListComponent } from './components/page/user-task-list/user-task-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'userlogin', pathMatch: 'full' },
  { path: 'userlogin', component: LoginComponent },
  { path: 'userregister', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'tasklist', component: TasklistComponent, canActivate: [AuthGuard]},
  { path: 'task-details/:id', component: TaskDetailsComponent, canActivate: [AuthGuard] },
  { path: 'tasklist/:type', component: UserTaskListComponent, canActivate: [AuthGuard]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
