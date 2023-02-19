import { Component, Injector, OnInit, AfterViewInit } from '@angular/core';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'app-layout', // Optional: Add a selector if needed
    templateUrl: './layout.component.html',
    animations: [accountModuleAnimation()],
    styleUrls: ['./layout.component.less'],
})
export class LayoutComponent extends AppComponentBase implements OnInit, AfterViewInit {
    submitting = false;
    isMultiTenancyEnabled: boolean;

    constructor(injector: Injector) {
        super(injector);
        this.isMultiTenancyEnabled = this.multiTenancy.isEnabled;
    }

    ngOnInit(): void {
        // Add initialization logic if needed
    }

    ngAfterViewInit(): void {
        // Add post-view initialization logic if needed
    }
}
