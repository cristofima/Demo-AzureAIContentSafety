import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Post } from '@/models/post.interface';
import { ApiService } from '@/services/api.service';
import { PostCreateComponent } from '../post-create/post-create.component';
import { environment } from '../../../environments/environment';
import { ErrorUtil } from '@/utils/error.util';

@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [CommonModule, PostCreateComponent],
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit {

  posts: Post[] = [];
  storageBaseUrl = environment.storageBaseUrl;
  showErrorMessage = false;
  errors: string[] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.showErrorMessage = false;
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

  setErrors(errors: string[]) {
    this.errors = errors;
    this.showErrorMessage = this.errors.length > 0;
  }

  clearErrors() {
    this.errors = [];
    this.showErrorMessage = false;
  }

  async deletePost(post: Post) {
    try {
      post.isDeleting = true;
      await lastValueFrom(this.apiService.deletePost(post.id));
      this.posts = this.posts.filter(p => p.id !== post.id);
    } catch (ex: any) {
      this.setErrors(ErrorUtil.getErrors(ex.error.errors));
    } finally {
      post.isDeleting = false;
    }
  }
}
