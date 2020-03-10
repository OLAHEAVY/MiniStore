import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import {
    NgxUiLoaderModule,
    NgxUiLoaderConfig,
    POSITION,
    SPINNER,
    PB_DIRECTION,
    NgxUiLoaderRouterModule,
    NgxUiLoaderHttpModule
} from "ngx-ui-loader";



const ngxUiLoaderConfig: NgxUiLoaderConfig = {
    bgsColor: "#4286f4",
    bgsPosition: POSITION.bottomCenter,
    bgsSize: 40,
    bgsType: SPINNER.rectangleBounce,
    pbDirection: PB_DIRECTION.leftToRight,
    pbThickness: 2,
    fgsType: "ball-spin-fade-rotating",
    fgsColor: "#a8ac2e",
    bgsOpacity: 0.9,
    pbColor: "#a8ac2e"
};

@NgModule({
    imports: [CommonModule,
             ReactiveFormsModule,
             FormsModule,
             NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
             HttpClientModule
           ],
    declarations: [],
    exports:[CommonModule,
            FormsModule,
            ReactiveFormsModule],
    providers: []
})

export class SharedModule{}