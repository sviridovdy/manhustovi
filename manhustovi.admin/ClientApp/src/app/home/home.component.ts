import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router) {
  }

  async ngOnInit() {
    const resposne = await fetch('/api/status');
    if (resposne.status === 401) {
      this.router.navigate(['/signin']);
    }
  }

  async onLogout() {
    const response = await fetch('/api/signout', {
      method: 'POST'
    });
    const content = await response.json();
    if (content.success) {
      this.router.navigate(['/signin']);
    }
  }

  onNewPost() {
    this.router.navigate(['/new'])
  }
}
