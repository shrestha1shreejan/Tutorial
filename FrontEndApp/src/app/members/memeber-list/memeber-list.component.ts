import { DataService } from '../../_services/data.service';
import { User } from '../../_models/User';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-memeber-list',
  templateUrl: './memeber-list.component.html',
  styleUrls: ['./memeber-list.component.css']
})
export class MemeberListComponent implements OnInit {

  users: User[];

  constructor(private dataService: DataService, private altertify: AlertifyService) { }

  ngOnInit() {
    this.getUsers();
  }


  getUsers() {
    this.dataService.getUsers().subscribe((users: User[]) => {
      this.users = users;
    }, error => {
      this.altertify.error(error);
    });
  }
}
