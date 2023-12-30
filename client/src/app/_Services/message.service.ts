import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MessageQueryParams } from '../_Models/QueryParams/MessageQueryParams';
import { PaginationResult } from '../_Models/PaginationModel';
import { BehaviorSubject, map, take } from 'rxjs';
import { MessageResponseModel } from '../_Models/MessageResponseModel';
import { MessageRequestModel } from '../_Models/Request/MessageRequestModel';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { UserModel } from '../_Models/UserModel';
import { GroupModel } from '../_Models/ChatGroupModels';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseurl = environment.ApiUrl
  hubUrl = environment.hubUrl
  private hubConnection?: HubConnection
  private currentMessageSource = new BehaviorSubject<MessageResponseModel[]>([]);
  messagesThread$ = this.currentMessageSource.asObservable();

  constructor(private http: HttpClient) { }

  createHubConnection(currentUser: UserModel, reciverUserName: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + `message?user=${reciverUserName}`,
        { accessTokenFactory: () => currentUser.Token })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));


    this.hubConnection.on("GetMessagesThread", (messages: MessageResponseModel[]) => {
      this.currentMessageSource.next(messages);
    })



    this.hubConnection.on("UpdatedGroup", (group: GroupModel) => {

      if (group.Connections.some(x => x.UserName === reciverUserName)) {
        this.messagesThread$.pipe(take(1)).subscribe({
          next: messages => {
            messages.forEach(message => {
              if (!message.DateRead) {
                message.DateRead = new Date(Date.now());
              }
            })
            this.currentMessageSource.next([...messages]);
          }
        })
      }
    })


    this.hubConnection.on("NewMessage", message => {
      console.log(message)
      this.messagesThread$.pipe(take(1)).subscribe({
        next: messages => {
          if (messages && message) {
            this.currentMessageSource.next([...messages, message]);
          }
        }
      })
    })



  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }






  getMessages(prms: MessageQueryParams) {
    return this.GetpaginatedResult<MessageResponseModel[]>(this.baseurl + "messages", this.GetPaginatedHeader(prms))

  }

  getMessagesThread(userName: string) {
    return this.http.get<MessageResponseModel[]>(this.baseurl + "messages/thread/" + userName);
  }


  async sendMessage(message: MessageRequestModel) {
    //invoke: returns promise. and by placing Async: we force it to send promise
    return this.hubConnection?.invoke("NewMessage", message).catch(error => {

      console.log(error)
    });
  }


  deleteMessage(id: number) {
    return this.http.delete(this.baseurl + `messages/${id}`)
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



  private GetPaginatedHeader(prms: MessageQueryParams) {
    var params = new HttpParams();
    params = params.append("PageNumber", prms.PageNumber);
    params = params.append("PageSize", prms.PageSize);
    params = params.append("Container", prms.Container);

    return params;
  }
}
