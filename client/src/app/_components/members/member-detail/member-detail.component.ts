import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  Member: MemberModel | undefined;
  galleryOptions: NgxGalleryOptions[]=[];
  galleryImages: NgxGalleryImage[] = [];


  constructor(private memberService: MembersService, private route: ActivatedRoute) { }
  ngOnInit(): void {
    this.LoadMember();
    this.galleryOptions = [
      {
        width: "500px",
        height: "500px",
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }

    ];
    
  }



  Getimages() {
  if(!this.Member?.Photos) return [];
  const imageUrl=[]
  for (const photo of this.Member.Photos) {
    imageUrl.push({
      small: photo.Url,
      medium: photo.Url,
      big: photo.Url,
    })
  }
return imageUrl
  }


  LoadMember() {
    const UserName = this.route.snapshot.paramMap.get("UserName");
    if (!UserName) return;
    this.memberService.GetMember(UserName).subscribe({
      next: member => {
        this.Member = member;
        this.galleryImages=this.Getimages()
      }
    });
  }

}
