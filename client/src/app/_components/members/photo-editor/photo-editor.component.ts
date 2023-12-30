import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';

import { take } from 'rxjs';
import { MemberModel } from 'src/app/_Models/MemberModel';
import { PhotoModel } from 'src/app/_Models/PhotoModel';
import { UserModel } from 'src/app/_Models/UserModel';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';
import { PhotoService } from 'src/app/_Services/photo.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: MemberModel | undefined;
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.ApiUrl;
  user: UserModel | undefined;


  constructor(
    private accountService: AccountService,
      private toast: ToastrService,
      private photoService:PhotoService

      ) {
    this.accountService.CurrentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })
  }
  ngOnInit(): void {
    this.initializeUploader()

  }
  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }


  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + "photos/add-photo",
      authToken: 'Bearer ' + this.user?.Token,
      isHTML5: true,
      allowedFileType: ["image"],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    // this.uploader.onAfterAddingFile=(file)=>{
    //   file.withCredentials=false
    // }
    this.uploader.onAfterAddingFile = (fileItem: any) => {
      fileItem.withCredentials = false;
    };


    this.uploader.onSuccessItem = (item, Response, status, Headers) => {
      if (Response) {
        var photo = JSON.parse(Response);
        this.member?.Photos.push(photo);
        // if (photo.IsMain && this.user && this.member) {
        //   this.user.PhotoUrl = photo.Url;
        //   this.member.PhotoUrl = photo.Url;
        //   this.accountService.SetCurrentUser(this.user)
        // }
      }
    };



  }


  setMainPhoto(photo: PhotoModel) {
    this.photoService.SetMainphoto(photo.Id).subscribe({
      next: _ => {
        if (this.member && this.user) {
          this.user.PhotoUrl = photo.Url;
          this.member.PhotoUrl = photo.Url;
          this.accountService.SetCurrentUser(this.user);

          this.member.Photos.forEach(e => {
            if (e.IsMain && e.Id != photo.Id) e.IsMain = false;
            if (e.Id == photo.Id) e.IsMain = true;
          });
        }
      }
    })
  }

  deletePhoto(photoId: number) {
    this.photoService.DeletePhoto(photoId).subscribe({

      next: res => {

        if (this.member) {
          this.member.Photos = this.member.Photos.filter(x => x.Id !== photoId);
        }
        this.toast.success("Image Deleted", "Success")

      },
      error: error => console.log(error)


    })
  }


}






