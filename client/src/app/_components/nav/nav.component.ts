import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../_Services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from 'src/app/_Services/message.service';
import { take } from 'rxjs';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // LoggedIn = false;
  model: any = {}
  messageCount="0"
  ngOnInit() {
   this.messageService.unReadMessages.pipe(take(1)).subscribe({
    next: msgs=>{
      
    if(msgs.length!=0)
      this.messageCount="18"
      if(msgs.length>=50)
      this.messageCount+="+"

    }
   })
   }
  constructor(
    public accountService: AccountService,
    public messageService:MessageService,
    private Route:Router,
    public toast:ToastrService
    
    ) { }

 


  login() {
    this.accountService.LogIn(this.model).subscribe({
      next: _=>{
        this.Route.navigateByUrl('/members'),
        this.toast.success("Login Success","Welcome")
        this.model={}
      },
     
    error: error => {
      this.toast.error(error.error)
    console.log(error.error);
  }
    
    })
  }

  LogOut() {
    this.accountService.Logout();
    this.Route.navigateByUrl('/')
   
  }

}
