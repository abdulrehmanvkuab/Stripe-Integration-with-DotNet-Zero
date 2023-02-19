import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {PhoneBookRoutingModule} from './phonebook-routing.module';
import {PhoneBookComponent} from './phonebook.component';

@NgModule({
    declarations: [PhoneBookComponent],
    imports: [AppSharedModule, PhoneBookRoutingModule]
})
export class PhoneBookModule {}
