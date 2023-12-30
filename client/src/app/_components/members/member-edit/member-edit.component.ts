import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { PhotoModel } from 'src/app/_Models/PhotoModel';
import { UserModel } from 'src/app/_Models/UserModel';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent {
 @ViewChild('editform') editform: NgForm|undefined;
 @HostListener("window:beforeunload", ['$event']) unloadNotification($event:any){
  if (this.editform?.dirty) {
    $event.returnValue=true;
    
  }
 }  
  member: MemberModel|undefined;
  user:UserModel|null=null;

  constructor(
    private accountService: AccountService,
    private memberService:MembersService,
    private toast:ToastrService)
    {
    this.accountService.CurrentUser$.pipe(take(1)).subscribe({
      next: user=>{
        this.user=user
        this.loadMember()
      }
    })
  }

  
loadMember(){
  if(this.user){
  this.memberService.GetMember(this.user?.UserName).subscribe({
    next: member=>{
      this.member=member

      //below code for fresh main photo if new  approved.
    if(this.member?.PhotoUrl){
      this.user!.PhotoUrl=this.member.PhotoUrl
      this.accountService.SetCurrentUser(this.user!)
    }}
  })
}
}

updateMember(){
  this.memberService.UpdateMember(this.editform?.value).subscribe({
    next:_=>{
      this.toast.success("Your Profile Updated")
      this.editform?.reset(this.member)
    }
  })
  
  
}



}
