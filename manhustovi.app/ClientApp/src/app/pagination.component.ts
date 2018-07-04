import {Component, EventEmitter, Input, Output} from "@angular/core";
import {PageRef} from "./models/page.ref";

@Component({
  selector: 'pagination-component',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})

export class PaginationComponent {
  @Input()
  pageRefs: PageRef[];

  @Input()
  pagesCount: number;

  @Output()
  loadPageEvent: EventEmitter<number> = new EventEmitter<number>();

  loadPage(pageNumber: number) {
    this.loadPageEvent.emit(pageNumber);
  }
}
