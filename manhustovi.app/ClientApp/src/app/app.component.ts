import {Component} from '@angular/core';

declare var PhotoSwipe;
declare var PhotoSwipeUI_Default;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent {
  public static openPhoto(items: any[], itemIndex: number) {
    const pswpElement = document.querySelectorAll('.pswp')[0];
    const options = {
      index: itemIndex
    };
    const gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);
    gallery.init();
  }
}
