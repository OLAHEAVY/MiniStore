import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ResponseModel } from 'src/app/_models/response';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: "app-send-emailconfirmation",
  templateUrl: "./send-emailconfirmation.component.html",
  styleUrls: ["./send-emailconfirmation.component.css"]
})
export class SendEmailconfirmationComponent implements OnInit {
  submitted: boolean = false;
  loading: boolean = false;
  resendLinkForm: FormGroup;
  user: any;

  constructor(  private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private toastr: ToastrService) {}

  ngOnInit() {
    this.resendLinkForm = this.fb.group({
      email: ["", [Validators.required, Validators.email]]
    });
  }

  resendLink(){
    this.submitted = true;
    if(this.resendLinkForm.valid){
      this.loading = true;
      this.user = Object.assign({}, this.resendLinkForm.value);

      this.authService.resendConfirmationLink(this.user).subscribe((x: ResponseModel) =>{
        if(!x.HasError){
          this.toastr.success(x.Message, 'Success');
        }else{
          this.toastr.error(x.Message, 'Error');
        }
        this.loading = false;
      },
      (err: HttpErrorResponse) =>{
        if (
          err.status === 401 ||
          err.status === 403 ||
          err.status === 500 ||
          err.status === 400
        ) {
          this.toastr.error(err.error || "An error has occured.", "Status");
          this.loading = false;
        }
      });
    }
  }
}

