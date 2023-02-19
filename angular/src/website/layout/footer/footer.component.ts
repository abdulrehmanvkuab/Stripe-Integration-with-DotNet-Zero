import { AbpSessionService } from 'abp-ng2-module';
import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';


@Component({
    selector: 'app-footer', // Add this
    templateUrl: './footer.component.html',
    animations: [accountModuleAnimation()],
    styleUrls: ['./footer.component.less'],
})
export class FooterComponent extends AppComponentBase implements OnInit, AfterViewInit {
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
