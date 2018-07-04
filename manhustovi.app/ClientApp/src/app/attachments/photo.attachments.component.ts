import {Component, Input} from '@angular/core';
import {PhotoAttachment} from '../models/photo.attachment';
import {AppComponent} from '../app.component';

@Component({
  selector: 'photo-attachments-component',
  templateUrl: './photo.attachments.component.html',
  styleUrls: ['./photo.attachments.component.css']
})

export class PhotoAttachmentsComponent {
  @Input()
  photoAttachments: PhotoAttachment[];

  public openPhoto(photo: PhotoAttachment) {
    const items = this.photoAttachments.map(p => p.createSizedInstance(1));
    const itemIndex = this.photoAttachments.indexOf(photo);
    AppComponent.openPhoto(items, itemIndex);
  }

  public isPanoramaPhoto(photo: PhotoAttachment): boolean {
    return photo.width > photo.height * 2;
  }
}
