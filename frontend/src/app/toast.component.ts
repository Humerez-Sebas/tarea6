import { Component } from '@angular/core';
import { NgFor } from '@angular/common';
import { ToastService } from './toast.service';

@Component({
  selector: 'app-toast-container',
  standalone: true,
  imports: [NgFor],
  template: `
    <div class="toast" *ngFor="let t of toasts">{{ t }}</div>
  `,
  styleUrl: './toast.component.css'
})
export class ToastContainerComponent {
  toasts: string[] = [];

  constructor(toast: ToastService) {
    toast.toast$.subscribe(msg => {
      this.toasts.push(msg);
      setTimeout(() => this.toasts.shift(), 3000);
    });
  }
}
