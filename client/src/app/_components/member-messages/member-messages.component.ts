import { ImplicitReceiver } from '@angular/compiler';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MessageResponseModel } from 'src/app/_Models/MessageResponseModel';
import { MessageRequestModel } from 'src/app/_Models/Request/MessageRequestModel';
import { MessageService } from 'src/app/_Services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  @ViewChild('messageForm') messageForm?: NgForm
  @Input() memberUserName?: string
  @Input() messages: MessageResponseModel[] = []
  messageContent: string | undefined

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {

  }

  sendMessage() {
    if (this.memberUserName && this.messageContent && this.messageContent.length > 0) {
      var message: MessageRequestModel = { RecipientUserName: this.memberUserName, Content: this.messageContent }
      this.messageService.sendMessage(message).pipe().subscribe({
        next: res => {
          this.messages.push(res);
          this.messageForm?.reset()
        }
      })
    }
  }
//abcabc





}