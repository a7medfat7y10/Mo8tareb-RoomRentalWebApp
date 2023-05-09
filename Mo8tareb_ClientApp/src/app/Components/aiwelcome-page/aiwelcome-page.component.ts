import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-aiwelcome-page',
  templateUrl: './aiwelcome-page.component.html',
  styleUrls: ['./aiwelcome-page.component.css']
})
export class AIWelcomePageComponent {
  constructor(private translate: TranslateService){}
  isRtl(): boolean {
    const currentLang = this.translate.currentLang;
    return currentLang === 'ar';
  }
}
