export class Photo {
    public showControls: false;

    constructor(public file: File, public source: string | ArrayBuffer, public previewSrc: string) {
    }
}
