import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { ResponseModel } from 'src/app/_models/response';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  submitted: boolean = false;
  loading: boolean = false;
  forgotPasswordForm: FormGroup;
  user: any;

  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.forgotPasswordForm = this.fb.group({
      email: ["", [Validators.required, Validators.email]]
    });
  }

  forgotPassword(){
    this.submitted = true;
    if(this.forgotPasswordForm.valid){
      this.loading = true;

      this.user = Object.assign({}, this.forgotPasswordForm.value);
     this.authService.sendPasswordResetLink(this.user).subscribe((x : ResponseModel) =>{
       if(!x.HasError){
         this.toastr.success(x.Message, 'Success');
       }else{
         this.toastr.error(x.Message, 'Error')
       }
       this.loading = false;
     },
     (err: HttpErrorResponse) => {
       if(
        err.status === 401 ||
        err.status === 403 ||
        err.status === 500 ||
        err.status === 400
       ){
         this.toastr.error(err.error || "An error has occured.", "Status");
         this.loading = false;
       }
     })
    }
  }

}
