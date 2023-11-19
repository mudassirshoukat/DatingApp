import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent  implements OnInit{
  members$: Observable<MemberModel[]>|undefined
  ngOnInit(): void {
    this.members$=this.memberservice.GetMembers()
  }
  constructor(private memberservice: MembersService) { }


}
