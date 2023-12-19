import { Component, OnInit } from '@angular/core';
import { map, take } from 'rxjs';
import { LikeMembersModel } from 'src/app/_Models/LikeMembersModel';
import { LikeQueryParams } from 'src/app/_Models/QueryParams/LikeQueryParams';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { PaginationModel } from 'src/app/_Models/PaginationModel';
import { LikeService } from 'src/app/_Services/like.service';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit  {
  members:LikeMembersModel[]|undefined
  pagination:PaginationModel|undefined
  likeprms:LikeQueryParams|undefined

constructor(private likeService: LikeService){
  this.likeprms=this.likeService.getQueryPrms();
  
}
  ngOnInit(): void {
    this.loadLikes()
  }

loadLikes(){
  if (this.likeprms) {
    

  this.likeService.setQueryPrms(this.likeprms);
  
  this.likeService.getLikes().pipe().subscribe({
    next:res=>{
      if(res.Result&&res.Pagination){
        this.members=[]
        this.pagination=undefined
        this.members=res.Result
        this.pagination=res.Pagination
      }

    }
  })
}

}



pageChanged(event: any) {
  if (this.likeprms && this.likeprms.PageNumber !== event.page) {

    this.likeprms.PageNumber = event.page
    this.likeService.setQueryPrms(this.likeprms);
    this.loadLikes()

  }

}

}
