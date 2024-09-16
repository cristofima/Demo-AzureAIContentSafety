import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '@/models/post.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl = 'https://localhost:44310/api';

  constructor(private http: HttpClient) { }

  public getPosts() {
    return this.http.get<Post[]>(`${this.baseUrl}/Posts`);
  }

  public createPost(data: FormData) {
    return this.http.post<Post>(`${this.baseUrl}/Posts`, data);
  }

  public deletePost(id: string) {
    return this.http.delete(`${this.baseUrl}/Posts/${id}`);
  }
}
