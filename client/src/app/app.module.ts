

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from '@angular/forms'; // Import FormsModule if needed
import { ReactiveFormsModule } from '@angular/forms'; // Import ReactiveFormsModule if needed

@NgModule({
  declarations: [
    AppComponent,
    // Include other components here
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule, // Include if you plan to use template-driven forms
    ReactiveFormsModule, // Include if you plan to use reactive forms
    // Include other modules here
  ],
  providers: [
    // Include services here
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }


// import { NgModule } from '@angular/core';
// import { BrowserModule } from '@angular/platform-browser';

// import { AppRoutingModule } from './app-routing.module';
// import { AppComponent } from './app.component';
// import {HttpClientModule} from "@angular/common/http"

// @NgModule({
//   declarations: [
//     AppComponent
//   ],
//   imports: [
//     BrowserModule,
//     AppRoutingModule,
//     HttpClientModule
//   ],  
//   providers: [],
//   bootstrap: [AppComponent]
// })
// export class AppModule { }
