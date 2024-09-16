export interface Post {
  id: string;
  text: string;
  imagePath: string;
  textIsHarmful: boolean;
  imageIsHarmful: boolean;
  createdAt: Date;
  isDeleting: boolean;
}
