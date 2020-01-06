import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../_services/data.service';
import { User } from '../../_models/User';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../_services/alertify.service';
import { Pagination, PaginationResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-memeber-list',
  templateUrl: './memeber-list.component.html',
  styleUrls: ['./memeber-list.component.css']
})
export class MemeberListComponent implements OnInit {

  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{ value: 'male', display: 'Male' }, { value: 'female', display: 'Female' }];
  userParams: any = {};
  pagination: Pagination;

  // constructor(private dataService: DataService, private altertify: AlertifyService, private route: ActivatedRoute) { }
  constructor(private route: ActivatedRoute, private dataService: DataService, private alertify: AlertifyService) { }

  ngOnInit() {
    // this.getUsers();

    this.route.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });

    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
  }


  resetFilters() {
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.getUsers();
  }


  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getUsers();
    console.log(this.pagination.currentPage);
  }

  // using resolver instead of this
  // getting the users om page change
  getUsers() {
    this.dataService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: PaginationResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error(error);
      });
  }
}
