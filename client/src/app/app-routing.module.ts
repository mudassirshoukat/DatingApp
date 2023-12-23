import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './_components/home/home.component';
import { MemberListComponent } from './_components/members/member-list/member-list.component';
import { MemberDetailComponent } from './_components/members/member-detail/member-detail.component';
import { ListsComponent } from './_components/lists/lists.component';
import { MessagesComponent } from './_components/messages/messages.component';
import { authGuard } from './_Gaurds/auth.guard';
import { TestErrorComponent } from './_components/errors/test-error/test-error.component';
import { NotFoundComponent } from './_components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './_components/errors/server-error/server-error.component';
import { MemberEditComponent } from './_components/members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_Gaurds/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './_Resolvers/member-detailed.resolver';
import { AdminPanelComponent } from './_components/admin/admin-panel/admin-panel.component';
import { adminGuard } from './_Gaurds/admin.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'error', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '', 
    runGuardsAndResolvers: 'always',
    canActivate:[authGuard],
    children: [
      { path: 'members', component: MemberListComponent, },
      { path: 'members/:UserName', component: MemberDetailComponent ,resolve:{member:memberDetailedResolver}},
      { path: 'member/edit', component: MemberEditComponent,canDeactivate:[preventUnsavedChangesGuard]},
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
      { path: 'admin', component: AdminPanelComponent,canActivate:[adminGuard] },
    ]
  },

  { path: '**', component: NotFoundComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
