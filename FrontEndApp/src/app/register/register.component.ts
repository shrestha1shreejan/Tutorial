import { Router } from '@angular/router';
import { User } from 'src/app/_models/User';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() isCancelledFromRegister = new EventEmitter();
  user: User;

  // ngx-bootstrap date-picker
  // making BsDatepickerConfig as partial to only implemet the propeties we want and not care about all its required prop
  bsConfig: Partial<BsDatepickerConfig>;

  registrationForm: FormGroup;

  constructor(private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    // this.registrationForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.passwordMatcherValidator);
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegistrationForm();
  }

  register() {

    if (this.registrationForm.valid) {
      this.user = Object.assign({}, this.registrationForm.value);
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registration Successful');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']); // login and send the user to the memebers page
        });
      });
    }

    console.log(this.registrationForm.value);
    // this.authService.register(this.model).subscribe(() => {
    //   this.alertify.success('Registration Successful');
    // }, error => {
    //   this.alertify.error(error);
    // });
  }


  // create register form
  createRegistrationForm() {
    this.registrationForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatcherValidator });
  }

  // Password Matcher
  passwordMatcherValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { mismatch: true };
  }


  cancel() {
    this.isCancelledFromRegister.emit(false);
  }
}
