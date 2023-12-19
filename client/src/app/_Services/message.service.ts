import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MessageQueryParams } from '../_Models/QueryParams/MessageQueryParams';
import { PaginationResult } from '../_Models/PaginationModel';
import { map } from 'rxjs';
import { MessageResponseModel } from '../_Models/MessageResponseModel';
import { MessageRequestModel } from '../_Models/Request/MessageRequestModel';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseurl = environment.ApiUrl

  constructor(private http: HttpClient) {

  }


  getMessages(prms: MessageQueryParams) {
    return this.GetpaginatedResult<MessageResponseModel[]>(this.baseurl + "messages", this.GetQueryHeader(prms))

  }

  getMessagesThread(userName: string) {
    return this.http.get<MessageResponseModel[]>(this.baseurl + "messages/thread/" + userName);
  }


sendMessage(message: MessageRequestModel){
  return this.http.post<MessageResponseModel>(this.baseurl+'messages',message)
 }


 deleteMessage(id:number){
  return this.http.delete(this.baseurl+`messages/${id}`)
 }

  private GetpaginatedResult<T>(url: string, params: HttpParams) {

    const paginatedResult: PaginationResult<T> = new PaginationResult<T>;

    return this.http.get<T>(url, { observe: 'response', params }).pipe(

      map(response => {
        if (response.body) {
          paginatedResult.Result = response.body;

        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.Pagination = JSON.parse(pagination);
        }

        return paginatedResult;
      })

    );
  }



  private GetQueryHeader(prms: MessageQueryParams) {
    var params = new HttpParams();
    params = params.append("PageNumber", prms.PageNumber);
    params = params.append("PageSize", prms.PageSize);
    params = params.append("Container", prms.Container);

    return params;
  }
}
