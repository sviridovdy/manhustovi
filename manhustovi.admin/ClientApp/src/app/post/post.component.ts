import * as Sortable from 'sortablejs';
import { Router } from '@angular/router';
import { Component, ViewChild, OnInit } from '@angular/core';
import { CreatePostRequest } from '../models/createPostRequest';
import { Photo } from '../models/photo';
import { UploadService } from '../upload.service';

@Component({
    selector: 'app-post',
    templateUrl: './post.component.html',
    styleUrls: ['./post.component.css']
})

export class PostComponent implements OnInit {

    showDaySelector = false;

    showAddPopup = false;

    showVideoPopup = false;

    postDate: Date;

    text = 'M: ';

    photos: Photo[] = [];

    videoUrl: string;

    videoPreviewUrl: string;

    @ViewChild('dateSelector') dateSelector;

    @ViewChild('imagesContainer') imagesContainer;

    @ViewChild('imageAttachInput') imageAttachInput;

    get dayNumber(): number {
        const startDate = new Date(2016, 2, 9);
        return Math.round((this.postDate.valueOf() - startDate.valueOf()) / 864E5);
    }

    setPostDate(date: string) {
        this.postDate = new Date(Date.parse(date));
        this.showDaySelector = false;
    }

    constructor(private router: Router, private uploadService: UploadService) {
        const now = new Date();
        this.postDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    }

    ngOnInit() {
        const options = {
            animation: 200
        };
        const sortable = Sortable.create(this.imagesContainer.nativeElement, options);
    }

    onPostCreate = () => {
        const createPostRequest = new CreatePostRequest('мангустові_usa', this.dayNumber, this.text, this.videoPreviewUrl);
        this.uploadService.createPostRequest = createPostRequest;
        this.uploadService.photoFiles = this.photos.map(p => p.file);
        this.router.navigate(['/upload']);
    }

    onDayClick = () => {
        this.dateSelector.nativeElement.click();
    }

    onAddImageClick = () => {
        this.showAddPopup = false;
        this.imageAttachInput.nativeElement.click();
    }

    onAddVideoClick = () => {
        this.showAddPopup = false;
        this.showVideoPopup = true;
    }

    onVideoUrlKeyPress = (event: KeyboardEvent) => {
        if (event.key === 'Enter') {
            this.showVideoPopup = false;
            const videoNumber = this.videoUrl.match('\/([0-9]+)')[1];
            this.videoPreviewUrl = `https://player.vimeo.com/video/${videoNumber}`;
        }
    }

    onAttachmentsChange = () => {
        for (const file of this.imageAttachInput.nativeElement.files) {
            const reader = new FileReader();
            reader.onload = () => {
                const image = new Image();
                image.onload = () => {
                    const width = 200;
                    const height = width * image.height / image.width;
                    const canvas = document.createElement('canvas');
                    canvas.width = width;
                    canvas.height = height;
                    canvas.getContext('2d').drawImage(image, 0, 0, width, height);
                    this.photos.push(new Photo(file, reader.result, canvas.toDataURL('image/jpeg')));
                };
                image.src = <string>reader.result;
            };

            reader.readAsDataURL(file);
        }
    }
}
