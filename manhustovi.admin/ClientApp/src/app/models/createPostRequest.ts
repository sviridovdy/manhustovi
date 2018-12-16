export class CreatePostRequest {
    constructor(public hashtag: string, public dayNumber: number, public text: string, public videoUrl: string) { }
}
