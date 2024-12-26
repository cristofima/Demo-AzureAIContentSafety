import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '@/models/post.interface';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl = environment.apiBaseUrl;
  private storageBaseUrl = environment.storageBaseUrl;

  constructor(private http: HttpClient) { }

  public getPosts() {
    return this.http.get<Post[]>(`${this.baseUrl}/Posts`)
      .pipe(map(posts => {
        return posts.map(post => {
          post.imagePath = `${this.storageBaseUrl}/images/${post.imagePath}`;
          post.imageIsBlurred = post.imageIsHarmful;
          return post;
        });
      }));
  }

  public createPost(data: FormData) {
    return this.http.post<Post>(`${this.baseUrl}/Posts`, data)
      .pipe(map(post => {
        post.imagePath = `${this.storageBaseUrl}/images/${post.imagePath}`;
        post.imageIsBlurred = post.imageIsHarmful;
        return post;
      }));
  }

  public deletePost(id: string) {
    return this.http.delete(`${this.baseUrl}/Posts/${id}`);
  }
}
