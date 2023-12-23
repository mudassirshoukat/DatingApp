import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserModel } from '../_Models/UserModel';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
private baseUrl:string=environment.ApiUrl
  constructor(private http:HttpClient) {

   }

   getUsersWithRoles(){
    return this.http.get<UserModel[]>(this.baseUrl+'admin/users-with-roles');
   }

   updateUserRoles(userName:string,roles:string[]){
    return this.http.post<string[]>(this.baseUrl+`admin/edit-roles/${userName}?roles=${roles}`,{});
   }
}
