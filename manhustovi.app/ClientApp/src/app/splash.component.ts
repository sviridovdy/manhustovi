import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-splash-component',
  templateUrl: './splash.component.html',
  styleUrls: ['./splash.component.css']
})

export class SplashComponent {

  @Output()
  public done: EventEmitter<any>;
  public showTopImage: boolean;
  public showBottomImage: boolean;

  constructor() {
    this.done = new EventEmitter();
    this.showTopImage = false;
    this.showBottomImage = true;
    setTimeout(this.startTransition, 500);
  }

  private startTransition = () => {
    this.showTopImage = true;
    setTimeout(this.stopTransition, 1000);
  };

  private stopTransition = () => {
    this.showBottomImage = false;
    setTimeout(this.completeTransition, 500);
  };

  private completeTransition = () => {
    this.done.emit(null);
  };
}
