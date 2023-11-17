import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../_Services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


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
  constructor(
    public accountService: AccountService,
    private Route:Router,
    public toast:ToastrService
    
    ) { }

 


  login() {
    this.accountService.LogIn(this.model).subscribe({
      next: _=>{
        this.Route.navigateByUrl('/members'),
        this.toast.success("Login Success","Welcome")
      },
     
    error: error => this.toast.error("Failed")
    })
  }

  LogOut() {
    this.accountService.Logout();
    this.Route.navigateByUrl('/')
   
  }

}
