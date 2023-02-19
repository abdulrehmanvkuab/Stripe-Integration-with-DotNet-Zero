import { AbpSessionService } from 'abp-ng2-module';
import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    templateUrl: './website.component.html',
    animations: [accountModuleAnimation()],
    styleUrls: ['./website.component.less'],
})
export class WebsiteComponent extends AppComponentBase implements OnInit, AfterViewInit {
    submitting = false;
    isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;

    constructor(
        injector: Injector,
     
    ) {
        super(injector);
    }

   

    ngOnInit(): void {
       
    }

    ngAfterViewInit(): void {
      
    }


}
