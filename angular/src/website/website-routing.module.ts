// import { NgModule } from '@angular/core';
// import { RouterModule, Routes } from '@angular/router';
// import { WebsiteComponent } from './home/website.component';


// const routes: Routes = [
//     {
//         path: 'home',
//         component: WebsiteComponent,
//         pathMatch: 'full',
//     },
// ];

// @NgModule({
//     imports: [RouterModule.forChild(routes)],
//     exports: [RouterModule],
// })
// export class WebsiteRoutingModule {}
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WebsiteComponent } from './home/website.component';
import { LayoutComponent } from './layout/layout/layout.component';
import { AboutComponent } from './about/about.component';
import { AnnouncementComponent } from './announcement/announcement.component';
import { ContactComponent } from './contact/contact.component';
import { DeveloperComponent } from './developer/developer.component';

const routes: Routes = [
  {
    path: '', // Empty path for the website module
    component: LayoutComponent,
    children: [
      {
        path: 'home',
        component: WebsiteComponent, // Home Page
        pathMatch: 'full',
      },
      {
        path: 'announcement',
        component: AnnouncementComponent, // Home Page
        pathMatch: 'full',
      },
      {
        path: 'about',
        component: AboutComponent, // Home Page
        pathMatch: 'full',
      },
      {
        path: 'developer',
        component: DeveloperComponent, // Home Page
        pathMatch: 'full',
      },
      {
        path: 'contact',
        component: ContactComponent, // Home Page
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WebsiteRoutingModule {}
