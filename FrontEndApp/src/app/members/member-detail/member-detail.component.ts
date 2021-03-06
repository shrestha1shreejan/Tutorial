import { AlertifyService } from './../../_services/alertify.service';
import { DataService } from './../../_services/data.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  // constructor(private dataService: DataService, private alertify: AlertifyService, private route: ActivatedRoute ) { }
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data.user;
    });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];


    this.galleryImages = this.getImages();

  }

  getImages() {
    const imageUrls = [];
    if (this.user.photos !== null) {
      for (const photo of this.user.photos) {
        imageUrls.push({
          small: photo.url,
          medium: photo.url,
          big: photo.url,
          description: photo.description
        });
      }
    }
    return imageUrls;
  }

  // getting this data from resolver now
  // getUserDetails() {
  //   this.dataService.getUser(this.route.snapshot.params.id).subscribe((data: User) => {
  //     this.user = data;
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }

}
