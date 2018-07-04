import {AfterViewChecked, Component, Input, OnInit} from '@angular/core';
import {Post} from './models/post';
import {Utils} from './utils';
import {Router} from '@angular/router';

declare var FB: any;

@Component({
  selector: 'post-component',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})

export class PostComponent implements OnInit, AfterViewChecked {

  private rendered = true;
  public isPostTruncated: boolean;
  public truncatedText: string;

  @Input()
  public post: Post;

  @Input()
  public showFullComments: boolean;

  public get postText(): string {
    return this.isPostTruncated ? this.truncatedText : this.post.text;
  }

  constructor(private router: Router) {

  }

  ngOnInit(): void {
    const lineIndex = this.post.text.indexOf('<hr>');
    if (this.post.text.length < 500) {
      this.isPostTruncated = false;
    } else {
      const index = this.post.text.indexOf('. ', 500 + (lineIndex === -1 ? 0 : lineIndex));
      if (index === -1) {
        this.isPostTruncated = false;
      } else {
        this.isPostTruncated = true;
        this.truncatedText = this.post.text.substring(0, index + 1);
      }
    }
  }

  ngAfterViewChecked(): void {
    if (!this.rendered) {
      this.rendered = true;
      FB.XFBML.parse(document.getElementById('root'));
    }
  }

  public navigateToPost(id: string): void {
    this.router.navigateByUrl(`/post/${id}`);
  }

  public getPostWeekDay(): string {
    return Utils.getWeekDayString(this.post.unixTimestamp);
  }

  public getPostDate(): string {
    return Utils.getDateString(this.post.unixTimestamp);
  }

  public expandComments(): void {
    this.rendered = false;
    this.showFullComments = true;
  }
}
