import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { MessageResponseModel } from 'src/app/_Models/MessageResponseModel';
import { MembersService } from 'src/app/_Services/members.service';
import { MessageService } from 'src/app/_Services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  //{static:true}: we can access this child component before loaded view
  @ViewChild("memberTabs", { static: true }) memberTabs?: TabsetComponent
  member: MemberModel ={} as MemberModel;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  activeTab?: TabDirective
  messages: MessageResponseModel[] = []


  constructor(private memberService: MembersService, private route: ActivatedRoute, private messageService: MessageService) { }
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
    if (this.activeTab.heading === "Messages") {
      this.loadMessages()
    }
  }


  loadMessages() {
    if (this.member?.UserName) {
      this.messageService.getMessagesThread(this.member.UserName).pipe().subscribe({
        next: res => {
          this.messages = res
        }
      })
    }}



}
