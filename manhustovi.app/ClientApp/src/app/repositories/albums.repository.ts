import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { plainToClass } from "class-transformer";
import { Album } from "../models/album";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class AlbumsRepository {

  constructor(private http: HttpClient) {
  }

  public getAlbum(id: string): Observable<Album> {
    return this.http.get(`/api/albums/${id}`)
      .map(json => plainToClass(Album, json as Object));
  }
}
