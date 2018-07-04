import {Component, Input} from "@angular/core";
import {VideoAttachment} from "../models/video.attachment";

@Component({
  selector: 'video-attachment-component',
  templateUrl: './video.attachment.component.html',
  styleUrls: ['./video.attachment.component.css']
})

export class VideoComponent {
  @Input()
  source: VideoAttachment;
}
