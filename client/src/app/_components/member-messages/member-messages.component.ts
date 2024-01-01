import { ImplicitReceiver } from '@angular/compiler';
import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MessageRequestModel } from 'src/app/_Models/Request/MessageRequestModel';
import { MessageService } from 'src/app/_Services/message.service';

@Component({
  changeDetection:ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  @ViewChild('messageForm') messageForm?: NgForm
  @Input() memberUserName?: string

  messageContent: string | undefined

  constructor(public messageService: MessageService) { }

  ngOnInit(): void {
    
      
    
  }


  sendMessage() {
  
    if (this.memberUserName && this.messageContent && this.messageContent.length > 0) {
      var message: MessageRequestModel = { RecipientUserName: this.memberUserName, Content: this.messageContent }
      this.messageService.sendMessage(message).then(() => {
        this.messageForm?.reset();
      })
    }
  }//
 



}
