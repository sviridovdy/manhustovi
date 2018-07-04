import {Component, Input} from "@angular/core";
import {AlbumAttachment} from "../models/album";
import {Router} from "@angular/router";

@Component({
  selector: 'album-attachment-component',
  templateUrl: './album.attachment.component.html',
  styleUrls: ['./album.attachment.component.css']
})

export class AlbumAttachmentComponent {
  @Input()
  albumAttachment: AlbumAttachment;

  constructor(private router: Router) {
  }

  public get imageUrl(): string {
    return `url(${this.albumAttachment.thumb.previewSrc})`;
  }

  public navigateToAlbum(id: string): void {
    this.router.navigateByUrl(`/album/${id}`);
  }
}
