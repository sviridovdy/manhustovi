import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { SignInComponent } from './signin/signin.component';
import { PostComponent } from './post/post.component';
import { ProgressBarComponent } from './progress-bar/progress-bar.component';
import { UploadComponent } from './upload/upload.component';
import { UploadService } from './upload.service';
import { SafeHtmlPipe, SafePipe } from './safe.pipe';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SignInComponent,
    PostComponent,
    UploadComponent,
    ProgressBarComponent,
    SafePipe,
    SafeHtmlPipe,

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'signin', component: SignInComponent, pathMatch: 'full' },
      { path: 'new', component: PostComponent, pathMatch: 'full' },
      { path: 'upload', component: UploadComponent, pathMatch: 'full' }
    ])
  ],
  providers: [UploadService],
  bootstrap: [AppComponent]
})
export class AppModule { }
