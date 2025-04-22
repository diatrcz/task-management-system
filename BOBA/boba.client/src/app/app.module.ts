import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { AppComponent } from './components/app.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { DashboardComponent } from './components/page/dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { TaskDetailsComponent } from './components/page/task-details/task-details.component';
import { HeaderComponent } from './components/frame/header/header.component';
import { TasklistComponent } from './components/page/tasklist/tasklist.component';
import { UserTaskListComponent } from './components/page/user-task-list/user-task-list.component';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';

@NgModule({ declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        DashboardComponent,
        TaskDetailsComponent,
        HeaderComponent,
        TasklistComponent,
        UserTaskListComponent
    ],
    bootstrap: [AppComponent], 
    imports: 
    [
        BrowserModule,
        AppRoutingModule,
        FormsModule
    ], 
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        AuthGuard,
        provideHttpClient(withInterceptorsFromDi()),
    ] })
export class AppModule { }
