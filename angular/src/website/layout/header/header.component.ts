import { Component, Injector, OnInit, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'app-header', // Add this
    templateUrl: './header.component.html', // Ensure this points to the correct HTML file
    styleUrls: ['./header.component.less'], // Ensure this points to the correct stylesheet
})
export class HeaderComponent extends AppComponentBase implements OnInit, AfterViewInit {
    submitting = false;
    isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
        // Initialization logic
    }

    ngAfterViewInit(): void {
        // After view init logic
    }
}
