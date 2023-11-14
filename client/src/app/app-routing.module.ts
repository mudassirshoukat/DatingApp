import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './_components/home/home.component';
import { MemberListComponent } from './_components/members/member-list/member-list.component';
import { MemberDetailComponent } from './_components/members/member-detail/member-detail.component';
import { ListsComponent } from './_components/lists/lists.component';
import { MessagesComponent } from './_components/messages/messages.component';
import { authGuard } from './_Gaurds/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: '', 
    runGuardsAndResolvers: 'always',
    canActivate:[authGuard],
    children: [
      { path: 'members', component: MemberListComponent, },
      { path: 'member/:id', component: MemberDetailComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
    ]
  },

  { path: '**', component: HomeComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
