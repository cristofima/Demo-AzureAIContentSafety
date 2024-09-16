import { Routes } from '@angular/router';
import { PostsComponent } from './components/posts/posts.component';

export const routes: Routes = [
  {
    path: 'posts',
    component: PostsComponent
  },
  {
    path: '',
    redirectTo: '/posts',
    pathMatch: 'full'
  }
];
