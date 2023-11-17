import { Component, OnInit } from '@angular/core';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent  implements OnInit{
  members: MemberModel[] = []
  list = ["a", 'd', 'g', 't', 'u']
  ngOnInit(): void {
    this.loadMembers()
  }
  constructor(private memberservice: MembersService) { }

  loadMembers() {
    this.memberservice.GetMembers().subscribe({
      next: res => this.members = res
    });
  }
}
