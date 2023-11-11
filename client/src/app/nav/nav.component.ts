import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_Services/account.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // LoggedIn = false;
  model: any = {}
  ngOnInit(): void {
   
   }
  constructor(public accountService: AccountService) { }

 


  login() {
    this.accountService.LogIn(this.model).subscribe({
      next: Response => {
        console.log(Response);
       
      },
      error: error => console.log(error)
    })
  }

  LogOut() {
    this.accountService.Logout();
   
  }

}
