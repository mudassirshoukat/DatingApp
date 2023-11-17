import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  RegisterMode=false;
// users:any;
constructor(private http:HttpClient){}

  ngOnInit(): void {
    // this.GetUsers()
  }

  // GetUsers() {
  //   this.http.get("https://localhost:44381/api/Users").subscribe({
  //     next: Response => this.users = Response,
  //     error: error => console.log(error),
  //     complete: () => console.log(this.users)

  //   })
  // }
  RegisterToggle(){
    this.RegisterMode=!this.RegisterMode;
  }

  cancelRegisterationMode(event:boolean){
    this.RegisterMode=event;
  }

}
