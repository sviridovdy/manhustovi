import {AfterViewChecked, Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {PostsRepository} from "./repositories/posts.repository";
import {Post} from "./models/post";

declare var FB: any;

@Component({
  selector: 'post-view-component',
  templateUrl: './post.view.component.html'
})

export class PostViewComponent implements OnInit, AfterViewChecked {
  private rendered: boolean = true;
  public post: Post;

  constructor(private route: ActivatedRoute, private postsRepository: PostsRepository) {
  }

  public ngOnInit(): void {
    let id: string = this.route.snapshot.params.id;
    let self = this;
    this.postsRepository.getPost(id).subscribe(value => {
      this.rendered = false;
      self.post = value.posts[0];
    });
  }

  ngAfterViewChecked(): void {
    if (!this.rendered) {
      this.rendered = true;
      FB.XFBML.parse(document.getElementById("root"));
    }
  }
}
