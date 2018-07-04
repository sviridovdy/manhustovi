export class PhotoAttachment {

  get src(): string {
    return this.mobileSrc;
  }

  public mobileSrc: string;
  public previewSrc: string;
  public width: number;
  public height: number;

  public createSizedInstance(scaleFactor: number): any {
    return {msrc: this.previewSrc, src: this.src, w: this.width * scaleFactor, h: this.height * scaleFactor};
  }
}
