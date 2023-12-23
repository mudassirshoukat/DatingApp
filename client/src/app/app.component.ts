
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_Services/account.service';
import { UserModel } from './_Models/UserModel';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingApp';


  constructor(private accountService: AccountService) { }

  

  ngOnInit(): void {
    this.SetCurrentUser()
  }

  SetCurrentUser() {
    const userString = localStorage.getItem("user");
    if (!userString)
      return;
    const user: UserModel = JSON.parse(userString);
    this.accountService.SetCurrentUser(user);

  }


}
