import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ErrorInterceptor } from './_Interceptors/error.interceptor';
import { SharedModule } from './_modules/shared.module';
import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { NavComponent } from './_components/nav/nav.component';
import { HomeComponent } from './_components/home/home.component';
import { RegisterComponent } from './_components/register/register.component';
import { ListsComponent } from './_components/lists/lists.component';
import { MessagesComponent } from './_components/messages/messages.component';
import { TestErrorComponent } from './_components/errors/test-error/test-error.component';
import { NotFoundComponent } from './_components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './_components/errors/server-error/server-error.component';
import { MemberDetailComponent } from './_components/members/member-detail/member-detail.component';
import { MemberListComponent } from './_components/members/member-list/member-list.component';
import { MemberCardComponent } from './_components/members/member-card/member-card.component';
import { JwtInterceptor } from './_Interceptors/jwt.interceptor';
import { MemberEditComponent } from './_components/members/member-edit/member-edit.component';
import { BusyLoadingInterceptor } from './_Interceptors/busy-loading.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    ListsComponent,
    MemberDetailComponent,
    MemberListComponent,
    MessagesComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
  
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule, 
    ReactiveFormsModule, 
    
   SharedModule
   
  ],
  providers: [
  {provide: HTTP_INTERCEPTORS,useClass: ErrorInterceptor,multi:true},
  {provide: HTTP_INTERCEPTORS,useClass: JwtInterceptor,multi:true},
  {provide: HTTP_INTERCEPTORS,useClass: BusyLoadingInterceptor,multi:true},
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }


