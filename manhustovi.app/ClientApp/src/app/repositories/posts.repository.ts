import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { plainToClass } from "class-transformer";
import { PostsResponse } from "../models/posts.response";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class PostsRepository {

  private readonly count: number = 10;

  constructor(private http: HttpClient) {
  }

  public getPost(id: string): Observable<PostsResponse> {
    return this.http.get(`/api/post/${id}`)
      .map(json => plainToClass(PostsResponse, json as Object));
  }

  public getPosts(pageNumber: number): Observable<PostsResponse> {
    let offset = pageNumber * this.count;
    return this.http.get(`/api/posts?offset=${offset}&count=${this.count}`)
      .map(json => plainToClass(PostsResponse, json as Object));
  }
}
