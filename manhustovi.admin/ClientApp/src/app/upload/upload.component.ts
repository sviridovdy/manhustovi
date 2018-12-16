import { NgZone, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { CreatePostResponse } from '../models/createPostResponse';
import { UploadService } from '../upload.service';

@Component({
    selector: 'app-upload',
    templateUrl: './upload.component.html',
    styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
    showUploadProgress = false;

    uploadProgressValue = 0;

    uploadProgressText: string;

    uploadResponseText: string;

    uploadEvents: string[] = [];

    constructor(private uploadService: UploadService, private zone: NgZone, private router: Router) { }

    ngOnInit() {
        if (!this.uploadService.createPostRequest) {
            this.router.navigate(['/new']);
            return;
        }

        const formData = new FormData();
        formData.append('post', JSON.stringify(this.uploadService.createPostRequest));
        for (const photo of this.uploadService.photoFiles) {
            formData.append('photo', photo);
        }

        this.uploadService.createPostRequest = null;
        this.uploadService.photoFiles = null;
        const xmlhttp = new XMLHttpRequest();
        xmlhttp.open('POST', '/api/posts', true);
        xmlhttp.upload.onprogress = this.onUploadProgress;
        xmlhttp.onreadystatechange = () => {
            if (xmlhttp.readyState !== 4) {
                return;
            }

            this.onUploadCompleted(xmlhttp);
        };
        xmlhttp.send(formData);
        this.showUploadProgress = true;
    }

    private onUploadProgress = (event: ProgressEvent) => {
        this.uploadProgressValue = Math.round(100 * event.loaded / event.total);
        this.uploadProgressText = `Uploaded ${this.humanFileSize(event.loaded)} / ${this.humanFileSize(event.total)}`;
    }

    private onUploadCompleted = (xmlhttp: XMLHttpRequest) => {
        if (xmlhttp.status === 200) {
            this.uploadResponseText = 'Post was sent';
            const response = JSON.parse(xmlhttp.response) as CreatePostResponse;
            const eventSource = new EventSource(`/api/postevents/${response.id}`);
            eventSource.onmessage = this.onUploadEvent;
        } else {
            this.uploadResponseText = `Post was NOT sent (status = ${xmlhttp.status})`;
        }

        setInterval(() => this.showUploadProgress = false, 2500);
    }

    private onUploadEvent = (event: MessageEvent) => {
        const d = JSON.parse(event.data);
        this.zone.run(() => {
            d.forEach((e: string) => this.uploadEvents.unshift(e));
        });
    }

    private humanFileSize(bytes: number): string {
        const thresh = 1024;
        if (Math.abs(bytes) < thresh) {
            return bytes + ' B';
        }
        const units = ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'];
        let u = -1;
        do {
            bytes /= thresh;
            ++u;
        } while (Math.abs(bytes) >= thresh && u < units.length - 1);
        return bytes.toFixed(1) + ' ' + units[u];
    }
}
