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
import { ToastrModule } from "ngx-toastr";
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';



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
             BrowserAnimationsModule,
             BrowserModule,
             NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
             HttpClientModule,
             NgxLoadingModule.forRoot({
                animationType: ngxLoadingAnimationTypes.circle,
                fullScreenBackdrop: true,
                backdropBackgroundColour: 'rgba(0,0,0,0.1)',
                backdropBorderRadius: '14px',
                primaryColour: '#536838',
                secondaryColour: '#a8ac2e',
                tertiaryColour: '#ffffff'
            }),
             ToastrModule.forRoot({
                preventDuplicates: false,
                timeOut: 10000,
                positionClass: "toast-top-center"
            })
            
           ],
    declarations: [],
    exports:[CommonModule,
            FormsModule,
            ReactiveFormsModule,
            ToastrModule,
            NgxLoadingModule,
            NgxUiLoaderModule],
    providers: []
})

export class SharedModule{}