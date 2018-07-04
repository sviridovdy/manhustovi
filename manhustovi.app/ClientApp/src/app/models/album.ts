import {PhotoAttachment} from "./photo.attachment";
import {Type} from "class-transformer";

export class AlbumAttachment {
  id: string;
  size: number;
  title: string;
  createdTimestamp: number;
  thumb: PhotoAttachment;
}

export class Album extends AlbumAttachment{
  @Type(() => PhotoAttachment)
  photos: PhotoAttachment[];
}
