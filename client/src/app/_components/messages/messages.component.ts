import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { MessageResponseModel } from 'src/app/_Models/MessageResponseModel';
import { PaginationModel } from 'src/app/_Models/PaginationModel';
import { MessageQueryParams } from 'src/app/_Models/QueryParams/MessageQueryParams';
import { MessageService } from 'src/app/_Services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages?: MessageResponseModel[]
  pagination?: PaginationModel
  queryParams: MessageQueryParams = new MessageQueryParams()
  loading = false


  constructor(public messageService: MessageService, private route: Router) {


  }
  ngOnInit(): void {
    this.loadMessages();
  }


  loadMessages() {
    this.loading = true
    this.messageService.getMessages(this.queryParams).pipe().subscribe({
      next: res => {
        this.messages = res.Result
        this.pagination = res.Pagination
        this.loading = false
      }
    })
  }


  pageChanged(event: any) {
    if (this.queryParams.PageNumber != event.page) {
      this.queryParams.PageNumber = event.page;
      this.loadMessages();

    }
  }

  deleteMessage(id: number,isUnRead:boolean) {

    this.messageService.deleteMessage(id).pipe(take(1)).subscribe({
      next: () => {
        // this.messageService.invokeUnReadMessages()
     if(isUnRead) {
      this.messageService.unReadMessages.pipe(take(1)).subscribe({
        next:unReadMessages=>{
          if(unReadMessages){
          unReadMessages=unReadMessages!.filter(unread=>unread.Id!==id)
          this.messageService.setUnReadMessages(unReadMessages);
          }
        } 
      })
     } else {
      this.messages = this.messages!.filter(message => message.Id !== id)
     }
     
    }
    })
  }


}
