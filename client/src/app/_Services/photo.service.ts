import { HttpClient, } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PhotoApprovalRequestModel } from '../_Models/Request/PhotoApprovalRequestModel';
import { PhotoModel } from '../_Models/PhotoModel';
import { PhotoApprovalResponsetModel } from '../_Models/Response/PhotoApprovalResponseModel';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  baseUrl = environment.ApiUrl
  constructor(private http: HttpClient) { }

  SetMainphoto(PhotoId: number) {
    return this.http.put(this.baseUrl + "photos/set-main-photo/" + PhotoId.toString(), {});
  }


  DeletePhoto(PhotoId: number) {
    return this.http.delete(this.baseUrl + "photos/delete-photo/" + PhotoId.toString());
  }


  photoApprove(model: PhotoApprovalRequestModel) {
    return this.http.put(this.baseUrl + "photos/photo-approval", model);
  }

  getPhotosToModerate(){
   return this.http.get<PhotoApprovalResponsetModel[]>(this.baseUrl+'photos/Photos-to-Moderate')
  }

}
