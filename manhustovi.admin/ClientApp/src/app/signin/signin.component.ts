import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SignInComponent {

  _oneTimeCode = '';

  wrongPassword = false;

  unknownError = false;

  get oneTimeCode() {
    return this._oneTimeCode;
  }

  set oneTimeCode(value) {
    this.wrongPassword = false;
    this.unknownError = false;
    this._oneTimeCode = value;
    if (value.length === 6) {
      this.submitOneTimeCode();
    }
  }

  constructor(private router: Router) {
  }

  async submitOneTimeCode() {

    const signInData = {
      code: this.oneTimeCode
    };

    this.oneTimeCode = '';
    this.unknownError = false;
    this.wrongPassword = false;

    try {
      const response = await fetch('/api/signin', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(signInData)
      });

      if (response.status === 200) {
        const content = await response.json();
        if (content.success) {
          this.router.navigate(['/']);
        } else {
          this.wrongPassword = true;
        }
      } else {
        this.unknownError = true;
      }
    } catch (Error) {
      this.unknownError = true;
    }
  }
}
