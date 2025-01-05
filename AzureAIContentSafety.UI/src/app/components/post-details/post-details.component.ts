import { Post } from '@/models/post.interface';
import { ApiService } from '@/services/api.service';
import { ErrorUtil } from '@/utils/error.util';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, model, output, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-post-details',
  imports: [CommonModule],
  templateUrl: './post-details.component.html',
  styleUrl: './post-details.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PostDetailsComponent {

  post = model.required<Post>();
  isDeleting = signal(false);
  deletedPost = output<Post>();
  errors = output<string[]>();

  constructor(private apiService: ApiService) { }

  toggleBlur() {
    this.post.update(p => ({ ...p, imageIsBlurred: !this.post().imageIsBlurred }));
  }

  async deletePost() {
    try {
      this.isDeleting.set(true);
      await lastValueFrom(this.apiService.deletePost(this.post().id));
      this.deletedPost.emit(this.post());
    } catch (ex: any) {
      this.errors.emit(ErrorUtil.getErrors(ex.error.errors));
    } finally {
      this.isDeleting.set(false);
    }
  }
}
