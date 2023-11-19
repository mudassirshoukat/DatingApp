import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MemberModel } from 'src/app/_Models/MemberModel';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent {
  @Input() member:MemberModel|undefined

  constructor(private route:Router){}
   
ProfileIconClick(UserName:string){
  this.route.navigateByUrl(`/members/${UserName}`)

}
}
