import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { HttpClientModule } from "@angular/common/http";
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';
import { BsDatepickerModule, BsDropdownModule, CarouselModule, ModalModule} from 'ngx-bootstrap';
import { ShopHomepageComponent } from './shop-homepage/shop-homepage.component';
import { AuthenticationModule } from '../shared/ordinary-layout/authentication/authentication.module';

@NgModule({
    imports: [CommonModule, 
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        NgxLoadingModule,
        AuthenticationModule,
        BsDropdownModule.forRoot(),
        BsDatepickerModule.forRoot(),
        CarouselModule.forRoot(),
        ModalModule.forRoot(),
        RouterModule.forChild([
            {path: 'shophomepage', component: ShopHomepageComponent},
        ])
   ],
    declarations: [ShopHomepageComponent],
    providers: [],
    exports: []
})

export class CustomerModule{}