import {Component, Input} from "@angular/core";

@Component({
  selector: 'comments-count-component',
  templateUrl: './comments.count.component.html',
  styleUrls: ['./comments.count.component.css']
})

export class CommentsCountComponent {
  @Input()
  public url: string;
}
