import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { PostsRepository } from './repositories/posts.repository';
import { PhotoGalleryComponent } from './photo.gallery.component';
import { AlbumComponent } from './album.component';
import { MainComponent } from './main.component';
import { SafeHtmlPipe, SafePipe } from './safe.pipe';
import { VideoComponent } from './attachments/video.attachment.component';
import { PaginationComponent } from './pagination.component';
import { AlbumAttachmentComponent } from './attachments/album.attachment.component';
import { PhotoAttachmentsComponent } from './attachments/photo.attachments.component';
import { AlbumsRepository } from './repositories/albums.repository';
import { FacebookModule } from 'ngx-facebook';
import { PostComponent } from './post.component';
import { PostViewComponent } from './post.view.component';
import { ToggleComponent } from './controls/toggle.component';
import { CommentsCountComponent } from './controls/comments.count.component';
import { SplashComponent } from './splash.component';
import { RootComponent } from './root.component';


@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    RootComponent,
    PostComponent,
    AlbumComponent,
    VideoComponent,
    ToggleComponent,
    SplashComponent,
    PostViewComponent,
    PaginationComponent,
    PhotoGalleryComponent,
    CommentsCountComponent,
    AlbumAttachmentComponent,
    PhotoAttachmentsComponent,
    SafePipe,
    SafeHtmlPipe

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    FacebookModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: RootComponent },
      { path: 'album/:id', component: AlbumComponent },
      { path: 'post/:id', component: PostViewComponent }
    ])
  ],
  providers: [
    PostsRepository,
    AlbumsRepository
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
