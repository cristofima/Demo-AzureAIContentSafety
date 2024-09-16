import { Post } from '@/models/post.interface';
import { ApiService } from '@/services/api.service';
import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-post-create',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './post-create.component.html',
  styleUrl: './post-create.component.scss'
})
export class PostCreateComponent implements OnInit {

  formGroup!: FormGroup;
  isLoading = false;

  @ViewChild('imageInput') imageInput!: ElementRef;
  @Output() postCreated = new EventEmitter<Post>();

  constructor(
    private apiService: ApiService,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit(): void {
    this.formGroup = this.formBuilder.group(
      {
        text: new FormControl('', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(1000)])),
        image: [null]
      }
    );
  }

  onFileSelected(event: any) {
    const selectedFile = event.target.files[0];
    this.formGroup.patchValue({
      image: selectedFile
    });

    this.formGroup.get('image')?.updateValueAndValidity();
  }

  async onSubmit() {
    this.isLoading = true;
    const formData = new FormData();
    formData.append('text', this.formGroup.get('text')?.value);
    const file = this.formGroup.get('image')?.value;
    if (file) {
      formData.append('image', file);
    }

    try {
      const post = await lastValueFrom(this.apiService.createPost(formData));
      this.postCreated.emit(post);
    } catch (error) {
      console.log('Error:', error);
    } finally {
      this.isLoading = false;
      this.formGroup.reset();
      if (this.imageInput) {
        this.imageInput.nativeElement.value = '';
      }
    }
  }
}
