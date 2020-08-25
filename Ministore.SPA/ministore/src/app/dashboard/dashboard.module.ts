import { NgModule } from "@angular/core";
import { DashboardComponent } from './dashboard.component';
import { CommonModule } from '@angular/common';
import { NgxLoadingModule } from 'ngx-loading';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        CommonModule,
        NgxLoadingModule,
        RouterModule.forChild([
            {
                path: '',
                component: DashboardComponent,
               
            }
        ])
    ],
    declarations: [DashboardComponent],
    exports: [],
    providers: []
})

export class DashboardModule{

}