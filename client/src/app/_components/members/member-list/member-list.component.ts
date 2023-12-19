import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { PaginationModel } from 'src/app/_Models/PaginationModel';
import { UserQueryParams } from 'src/app/_Models/QueryParams/UserQueryParams';
import { UserModel } from 'src/app/_Models/UserModel';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  // members$: Observable<MemberModel[]>|undefined
  members: MemberModel[] = []
  pagination: PaginationModel | undefined
  queryPrms: UserQueryParams | undefined;
  genderList=[{Value:"male",Display:"Males"},{Value:"female",Display:"Females"}];


  ngOnInit(): void {
    // this.members$=this.memberservice.GetMembers()
    this.loadMembers()
  }
  constructor(private memberservice: MembersService) {
    this.queryPrms=this.memberservice.getQueryPrms();
  
  }

  loadMembers() {
    if (this.queryPrms) {
      this.memberservice.setQueryPrms(this.queryPrms);
    this.memberservice.GetMembers(this.queryPrms).subscribe({
      next: res => {
        if (res.Result && res.Pagination) {
          this.pagination=undefined
          this.members=[]
          this.pagination = res.Pagination
          this.members = res.Result
        }
      }

    })
  }
  }

resetFilters(){
  
  this.queryPrms=this.memberservice.resetQueryPrms();
  this.loadMembers();


}

  pageChanged(event: any) {
    if (this.queryPrms && this.queryPrms.PageNumber !== event.page) {

      this.queryPrms.PageNumber = event.page
      this.memberservice.setQueryPrms(this.queryPrms);
      this.loadMembers()

    }

  }

}
