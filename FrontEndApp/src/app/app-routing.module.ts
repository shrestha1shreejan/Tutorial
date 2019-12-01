import { AuthGuard } from './_guards/auth.guard';
import { ListComponent } from './list/list.component';
import { MemeberListComponent } from './memeber-list/memeber-list.component';
import { MessagesComponent } from './messages/messages.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, CanActivate } from '@angular/router';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '', runGuardsAndResolvers: 'always', canActivate: [AuthGuard], children: [
      { path: 'messages', component: MessagesComponent },
      { path: 'members', component: MemeberListComponent },
      { path: 'lists', component: ListComponent }
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
