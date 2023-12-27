import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { StreamInvocationMessage } from '@microsoft/signalr';
import { PresenceService } from 'src/app/_Services/presence.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  RegisterMode = false;

  users: string[] = [];
  constructor() { }

  ngOnInit(): void {
    // this.presenceService.onlineUsers$.subscribe({
    //   next: users => this.users = users 
    // })
    // this.GetUsers()
  }


  RegisterToggle() {
    this.RegisterMode = !this.RegisterMode;
  }

  cancelRegisterationMode(event: boolean) {
    this.RegisterMode = event;
  }

}
