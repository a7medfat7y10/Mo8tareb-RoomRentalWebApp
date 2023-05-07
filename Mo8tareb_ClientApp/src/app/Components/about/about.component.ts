import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent {

  constructor(private translate: TranslateService){}
  isRtl(): boolean {
    const currentLang = this.translate.currentLang;
    return currentLang === 'ar';
  }

}
