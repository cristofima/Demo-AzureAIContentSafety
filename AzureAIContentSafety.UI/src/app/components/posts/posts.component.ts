import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Post } from '@/models/post.interface';
import { ApiService } from '@/services/api.service';
import { PostCreateComponent } from '../post-create/post-create.component';
import { ErrorUtil } from '@/utils/error.util';
import { InfiniteScrollDirective } from 'ngx-infinite-scroll';

@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [CommonModule, PostCreateComponent, InfiniteScrollDirective],
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit {

  posts: Post[] = [];
  showErrorMessage = false;
  errors: string[] = [];
  pageNumber = 0;
  isLoading = false;
  hasNextPage = false;
  showBottomButton = false;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.isLoading = false;
    this.showErrorMessage = false;
    this.posts = [];
    this.loadPosts();
  }

  private async loadPosts(pageNumber: number = 1) {
    if (this.isLoading) return;
    this.isLoading = true;

    const newPagination = await lastValueFrom(this.apiService.getPosts(pageNumber));
    this.posts = [...this.posts, ...newPagination.items];
    this.pageNumber = newPagination.pageNumber;
    this.hasNextPage = newPagination.hasNextPage;
    this.isLoading = false;
  }

  onWindowScroll() {
    this.showBottomButton = ((window.document.body.scrollHeight - window.innerHeight) - window.scrollY) > 350;
    if (this.hasNextPage) {
      this.loadPosts(this.pageNumber + 1);
    }
  }

  scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onPostCreated(post: Post) {
    this.posts = [post, ...this.posts];
  }

  toggleBlur(post: Post) {
    post.imageIsBlurred = !post.imageIsBlurred;
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
