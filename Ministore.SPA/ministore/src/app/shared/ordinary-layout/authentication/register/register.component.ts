import { Component, OnInit } from "@angular/core";
import { BsDatepickerConfig } from "ngx-bootstrap";
import {
  FormGroup,
  AbstractControl,
  FormBuilder,
  Validators
} from "@angular/forms";
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from "ngx-toastr";
import { ActivatedRoute, Router } from '@angular/router';

import { RegisterModel } from "src/app/_models/register-model";
import { AuthenticationService } from "src/app/_services/authentication.service";
import { ResponseModel } from "src/app/_models/response";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"]
})
export class RegisterComponent implements OnInit {
  // date picker
  bsConfig: Partial<BsDatepickerConfig>;
  registerForm: FormGroup;
  loading: boolean = false;
  submitted: boolean = false;
  user: RegisterModel;
  states: any;
  localGovernments: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    // BsDatePicker
    this.bsConfig = {
      containerClass: "theme-blue"
    };
    this.createRegisterForm();
    this.getAllStates();
  }

  //Reactive Form
  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        firstName: [
          "",
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20)
          ]
        ],
        lastName: [
          "",
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20)
          ]
        ],
        phone: ["", [Validators.required]],
        address: ["", [Validators.required]],
        gender: ["male"],
        email: ["", [Validators.required, Validators.email]],
        state: ["", Validators.required],
        city: ["", Validators.required],
        dateOfBirth: ["", Validators.required],
        password: [
          "",
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(11)
          ]
        ],
        confirmPassword: ["", Validators.required]
      },
      { validator: this.passwordMatchValidator }
    );
  }

  // custom validator for the password fields
  passwordMatchValidator(g: FormGroup) {
    return g.get("password").value === g.get("confirmPassword").value
      ? null
      : { mismatch: true };
  }
  register() {
    if (this.registerForm.valid) {
      this.submitted = true;
      this.loading = true;
      console.log(this.registerForm);
      this.user = Object.assign({}, this.registerForm.value);

      this.authService.register(this.user).subscribe((x: ResponseModel) => {
        //checking if the Has error is false or true
        if (!x.HasError) {
          this.toastr.success(x.Message, 'Success');
          this.router.navigate(['/login'])
        }else{
          this.toastr.warning(x.Message, 'Error');
        }
        this.loading = false;
      },
      (err: HttpErrorResponse) => {
        
        if (err.status === 401 || err.status === 403 || err.status === 500 || err.status === 400) {
          this.toastr.error(err.error || "An error has occured.", "Status");
          this.loading = false;
      }
    })
    }
  }

  getAllStates(){
    this.authService.getStates().subscribe((x:ResponseModel) => {
       if(!x.HasError){
          this.states = x.Result
       }else{
         this.toastr.error(x.Message, 'Error');
       }
    })
  }

  getAllLocalGovernments(id:string){
    this.authService.getLocalGovernments(id).subscribe((x:ResponseModel) => {
      if(!x.HasError){
        this.localGovernments = x.Result
      }else{
        this.toastr.error(x.Message, 'Error');
      }
    })
  }
}
