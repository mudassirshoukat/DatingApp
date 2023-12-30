import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { PhotoModel } from 'src/app/_Models/PhotoModel';
import { PhotoApprovalRequestModel } from 'src/app/_Models/Request/PhotoApprovalRequestModel';
import { PhotoApprovalResponsetModel } from 'src/app/_Models/Response/PhotoApprovalResponseModel';
import { PhotoService } from 'src/app/_Services/photo.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  photos: PhotoApprovalResponsetModel[] = []
  
  constructor(private photoService: PhotoService,private toast:ToastrService) { }
  ngOnInit(): void {

    this.getPhotosForModerate();
  }


  getPhotosForModerate() {
    this.photoService.getPhotosToModerate().pipe(take(1)).subscribe({
      next: photos => {
        this.photos = photos;
      }
    })
  }


  setPhotoApprove( id:number,approve:boolean){
    const requestModel: PhotoApprovalRequestModel = {
      Id: id,
      Approve: approve,
  };

  this.photoService.photoApprove(requestModel).pipe(take(1)).subscribe({
    next:(res:any)=>{
  
      this.toast.info(res.Message)
    this.photos=this.photos.filter(photo=>photo.Id!==id)

    },
  
  });
  }


}
