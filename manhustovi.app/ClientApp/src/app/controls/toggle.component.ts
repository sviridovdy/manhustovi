import {Component, EventEmitter, Output} from "@angular/core";

@Component({
  selector: 'toggle-component',
  templateUrl: './toggle.component.html',
  styleUrls: ['./toggle.component.css']
})

export class ToggleComponent {

  @Output()
  public onToggled = new EventEmitter<boolean>();

  public toggleState(value: Event): void {
    let state = (<HTMLInputElement>value.target).checked;
    this.onToggled.emit(state);
  }
}
