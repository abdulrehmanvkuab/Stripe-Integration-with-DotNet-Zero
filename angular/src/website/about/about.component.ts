import { AbpSessionService } from 'abp-ng2-module';
import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    templateUrl: './about.component.html',
    animations: [accountModuleAnimation()],
    styleUrls: ['./about.component.less'],
})
export class AboutComponent extends AppComponentBase implements OnInit, AfterViewInit {
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
