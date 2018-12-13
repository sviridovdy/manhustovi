import * as Sortable from 'sortablejs';

import { Router } from '@angular/router';
import { Component, ViewChild, OnInit } from '@angular/core';
import { CreatePostRequest } from '../models/createPostRequest';
import { Photo } from '../models/photo';

@Component({
    selector: 'app-post',
    templateUrl: './post.component.html',
    styleUrls: ['./post.component.css']
})

export class PostComponent implements OnInit {

    showDaySelector = false;

    showAddPopup = false;

    postDate: Date;

    text = 'M: ';

    photos: Photo[] = [];

    @ViewChild('dateSelector') dateSelector;

    @ViewChild('imagesContainer') imagesContainer;

    @ViewChild('imageAttachInput') imageAttachInput;

    result: string;

    get dayNumber(): number {
        const startDate = new Date(2016, 2, 9);
        return Math.round((this.postDate.valueOf() - startDate.valueOf()) / 864E5);
    }

    setPostDate(date: string) {
        this.postDate = new Date(Date.parse(date));
        this.showDaySelector = false;
    }


    constructor(private router: Router) {
        const now = new Date();
        this.postDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    }

    ngOnInit() {
        const options = {
            animation: 200
        };
        const sortable = Sortable.create(this.imagesContainer.nativeElement, options);
    }

    onDayClick() {
        this.dateSelector.nativeElement.click();
    }

    onAddImageClick() {
        this.imageAttachInput.nativeElement.click();
    }

    onAttachmentsChange() {
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

    async onPostCreate() {
        const createPostRequest = new CreatePostRequest('мангустові_usa', this.dayNumber, this.text);
        const formData = new FormData();
        formData.append('post', JSON.stringify(createPostRequest));
        for (const photo of this.photos) {
            formData.append('photo', photo.file);
        }

        try {
            const response = await fetch('/api/posts', {
                method: 'POST',
                body: formData
            });

            this.result = response.status === 200 ? 'post created' : `failed to create post. response code = ${response.status}`;
        } catch (e) {
            this.result = `failed to create post. error = ${e}`;
        }
    }
}
