import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { LikeMembersModel } from 'src/app/_Models/LikeMembersModel';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { MessageResponseModel } from 'src/app/_Models/MessageResponseModel';
import { UserModel } from 'src/app/_Models/UserModel';
import { AccountService } from 'src/app/_Services/account.service';
import { LikeService } from 'src/app/_Services/like.service';
import { MembersService } from 'src/app/_Services/members.service';
import { MessageService } from 'src/app/_Services/message.service';
import { PresenceService } from 'src/app/_Services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit,OnDestroy {
  //{static:true}: we can access this child component before loaded view
  @ViewChild("memberTabs", { static: true }) memberTabs?: TabsetComponent
  member: MemberModel = {} as MemberModel;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  activeTab?: TabDirective
  // messages: MessageResponseModel[] = []
  user?: UserModel


  constructor(
    private toast:ToastrService,
    private likeService:LikeService,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private messageService: MessageService,
    public presenceService: PresenceService) {
    this.accountService.CurrentUser$.pipe(take(1)).subscribe({
      next: res => {
        if (res) {
          this.user = res

        }
      }
    })
  }
  


  ngOnInit(): void {
    //data from route resolver
    this.route.data.subscribe({
      next: data => {
        this.member = data['member']
      }
    })

    // getting query param of routerlink
    this.route.queryParams.subscribe({
      next: prms => {
        prms['tab'] && this.selectTab(prms['tab'])
      }
    })


    this.galleryOptions = [
      {
        width: "500px",
        height: "500px",
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }];


    this.galleryImages = this.Getimages()
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }



  Getimages() {
    if (!this.member?.Photos) return [];
    const imageUrl = []
    for (const photo of this.member.Photos) {
      imageUrl.push({
        small: photo.Url,
        medium: photo.Url,
        big: photo.Url,
      })
    }
    return imageUrl
  }




  selectTab(heading: string) {
    if (this.member) {
      this.memberTabs!.tabs!.find(x => x.heading === heading)!.active = true

    }
  }



  onTabActivated(data: TabDirective) {
    this.activeTab = data
    if (this.activeTab.heading === "Messages"&&this.user) {
      
      this.messageService.createHubConnection(this.user,this.member.UserName)
    
    }
    else
    this.messageService.stopHubConnection();
  }


  AddLike(member:LikeMembersModel|MemberModel){
    this.likeService.addLike(member.UserName).pipe(take(1)).subscribe({
      next: _=>this.toast.success("You Have Liked "+member.KnownAs)
      
    })
  }


}
