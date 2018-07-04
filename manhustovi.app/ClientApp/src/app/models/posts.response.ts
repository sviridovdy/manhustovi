import {Post} from "./post";
import {Type} from "class-transformer";

export class PostsResponse {
  @Type(() => Post)
  public posts: Post[];
  public postsCount: number;
}
