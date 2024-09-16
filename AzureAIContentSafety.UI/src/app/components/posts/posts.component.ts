import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Post } from '@/models/post.interface';
import { ApiService } from '@/services/api.service';
import { PostCreateComponent } from '../post-create/post-create.component';

@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [CommonModule, PostCreateComponent],
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit {

  posts: Post[] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.apiService.getPosts().subscribe(posts => {
      this.posts = posts;
    });
  }

  onPostCreated(post: Post) {
    this.posts = [post, ...this.posts];
  }

  toggleBlur(post: Post) {
    post.imageIsHarmful = !post.imageIsHarmful;
  }

  async deletePost(post: Post) {
    try {
      post.isDeleting = true;
      await lastValueFrom(this.apiService.deletePost(post.id));
      this.posts = this.posts.filter(p => p.id !== post.id);
    } catch (error) {
      console.log('Error:', error);
    } finally {
      post.isDeleting = false;
    }
  }
}
