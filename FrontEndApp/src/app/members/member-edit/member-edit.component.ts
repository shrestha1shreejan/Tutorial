import { AuthService } from './../../_services/auth.service';
import { DataService } from './../../_services/data.service';
import { AlertifyService } from './../../_services/alertify.service';
import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/User';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  // accessing the form using the view child decorator
  @ViewChild('editForm') editForm: NgForm;
  user: User;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
              private dataService: DataService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data.user;
    });
  }


  // method to update user data
  updateUser() {
    console.log(this.user);
    const id = this.authService.decodedToken.nameid;
    this.dataService.updateUser(id, this.user).subscribe(next => {
      this.alertify.success('Successfully updated user information');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }
}

