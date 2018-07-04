import {AfterViewChecked, Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {AlbumsRepository} from './repositories/albums.repository';
import {Album} from './models/album';
import {Utils} from './utils';
import {PhotoAttachment} from './models/photo.attachment';
import {AppComponent} from './app.component';

declare var FB: any;

@Component({
  selector: 'album-component',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.scss']
})

export class AlbumComponent implements OnInit, AfterViewChecked {
  private rendered = true;
  private _showFullComments = false;
  public tinyPhotoMode = false;
  public album: Album;

  public get albumUri(): string {
    return `http://manhustovi.life/album/${this.album.id}`;
  }

  public get showFullComments(): boolean {
    return this._showFullComments;
  }

  public set showFullComments(value: boolean) {
    this.rendered = false;
    this._showFullComments = value;
  }

  constructor(private route: ActivatedRoute, private albumsRepository: AlbumsRepository) {
  }

  public ngOnInit(): void {
    const id: string = this.route.snapshot.params.id;
    const self = this;
    this.albumsRepository.getAlbum(id).subscribe(value => {
      self.rendered = false;
      self.album = value;
    });
  }

  public ngAfterViewChecked(): void {
    if (!this.rendered) {
      this.rendered = true;
      FB.XFBML.parse(document.getElementById('root'));
    }
  }

  public getAlbumWeekDay(): string {
    return Utils.getWeekDayString(this.album.createdTimestamp);
  }

  public getAlbumDate(): string {
    return Utils.getDateString(this.album.createdTimestamp);
  }

  public openPhoto(photo: PhotoAttachment) {
    const items = this.album.photos.map(p => p.createSizedInstance(1));
    const itemIndex = this.album.photos.indexOf(photo);
    AppComponent.openPhoto(items, itemIndex);
  }
}
