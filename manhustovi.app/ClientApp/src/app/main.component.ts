import {AfterViewChecked, Component, OnInit} from '@angular/core';
import 'rxjs/add/operator/map';
import {PostsRepository} from "./repositories/posts.repository";
import {PageRef} from "./models/page.ref";
import {Post} from "./models/post";

declare var FB: any;

@Component({
  selector: 'main-component',
  templateUrl: './main.component.html'
})

export class MainComponent implements OnInit, AfterViewChecked {

  private rendered: boolean = true;
  public posts: Post[];
  public pagesCount: number;
  private pageNumber: number = 0;
  public pageRefs = [];

  constructor(private postsRepository: PostsRepository) {
  }

  public ngOnInit(): void {
    this.loadPostsPage(0);
  }

  public ngAfterViewChecked(): void {
    if (!this.rendered) {
      this.rendered = true;
      FB.XFBML.parse(document.getElementById("root"));
    }
  }

  public loadPostsPage(number: number) {
    let self = this;
    this.postsRepository.getPosts(number).subscribe(postsResponse => {
      self.posts = postsResponse.posts;
      self.pagesCount = Math.ceil(postsResponse.postsCount / 10);
      self.pageNumber = number;
      let startIndex = Math.max(number - 3, 0);
      let newRefs = [];
      for (let i = 0; i < Math.min(6, self.pagesCount - startIndex); i++) {
        newRefs.push(new PageRef(startIndex + i + 1, startIndex + i === number));
      }
      this.rendered = false;
      self.pageRefs = newRefs;
    });
  }
}
