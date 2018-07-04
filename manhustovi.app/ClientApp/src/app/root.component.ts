import {Component} from '@angular/core';

@Component({
  selector: 'root-component',
  templateUrl: './root.component.html'
})

export class RootComponent {

  private readonly key = 'splash';
  public showSplash: boolean;
  public showMain: boolean;

  constructor() {
    this.showSplash = !localStorage.getItem(this.key);
    if (this.showSplash) {
      setTimeout(() => this.showMain = true, 500)
      localStorage.setItem(this.key, 'there you go, a cookie for you :)');
    }
    else {
      this.showMain = true;
    }
  }

  public onSplashDone() {
    this.showSplash = false;
  }
}
