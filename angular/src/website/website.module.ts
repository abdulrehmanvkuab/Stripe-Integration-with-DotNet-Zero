import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { WebsiteRoutingModule } from './website-routing.module';

import { AccountSharedModule } from '@account/shared/account-shared.module';
import { WebsiteComponent } from './home/website.component';

import { LayoutComponent } from './layout/layout/layout.component';
import { HeaderComponent } from './layout/header/header.component';
import { FooterComponent } from './layout/footer/footer.component';
import { AboutComponent } from './about/about.component';
import { AnnouncementComponent } from './announcement/announcement.component';
import { ContactComponent } from './contact/contact.component';
import { DeveloperComponent } from './developer/developer.component';

@NgModule({
    declarations: 
    [
        LayoutComponent,
    HeaderComponent,
    FooterComponent,
        WebsiteComponent,
        AboutComponent,
        AnnouncementComponent,
        ContactComponent,
        DeveloperComponent
    ],
    imports: [AppSharedModule, AccountSharedModule, WebsiteRoutingModule],
})
export class WebsiteModule {}
