import {PhotoAttachment} from "./photo.attachment";
import {Type} from "class-transformer";
import {VideoAttachment} from "./video.attachment";
import {LinkAttachment} from "./link.attachment";
import {AlbumAttachment} from "./album";

export class Post {
  public unixTimestamp: number;
  public text: string;
  public id: string;
  public dayNumber: number;
  public hashTag: string;
  @Type(() => PhotoAttachment)
  public photoAttachments: PhotoAttachment[];
  @Type(() => VideoAttachment)
  public videoAttachments: VideoAttachment[];
  @Type(() => LinkAttachment)
  public linkAttachment: LinkAttachment;
  @Type(() => AlbumAttachment)
  public albumAttachment: AlbumAttachment;
}
