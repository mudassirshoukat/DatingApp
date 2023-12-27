import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { LikeMembersModel } from 'src/app/_Models/LikeMembersModel';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { LikeService } from 'src/app/_Services/like.service';
import { PresenceService } from 'src/app/_Services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent {
  @Input() member:MemberModel|LikeMembersModel|undefined

  constructor(
    private route:Router,
    private likeService:LikeService,
    private toast:ToastrService,
    public presenceService:PresenceService ){}
   

AddLike(member:LikeMembersModel|MemberModel){
  this.likeService.addLike(member.UserName).pipe(take(1)).subscribe({
    next: _=>this.toast.success("You Have Liked "+member.KnownAs)
    
  })
}

ProfileIconClick(UserName:string){
  if (this.member && 'Introduction' in this.member)
  this.route.navigateByUrl(`/members/${UserName}`)

}
}
