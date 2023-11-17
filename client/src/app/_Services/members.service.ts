import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { MemberModel } from '../_Models/MemberModel';
import { environment } from 'src/environments/environment';
import { UserModel } from '../_Models/UserModel';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService  {
private baseurl:string=environment.ApiUrl

  constructor(private http:HttpClient) { }
 
GetMembers(){
  return this.http.get<MemberModel[]>(this.baseurl+"users");
  
}

GetMember(UserName:string){
  return this.http.get<MemberModel>(this.baseurl+"users/"+UserName)
}

}
