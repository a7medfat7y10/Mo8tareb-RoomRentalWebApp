import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent {

  constructor(private route:Router,private http:HttpClient,private translate: TranslateService) {

  }

  isRtl(): boolean {
    const currentLang = this.translate.currentLang;
    return currentLang === 'ar';
  }
  
  objData: any;

  collectdata(name:any,email:any,subject:any,message:any) {
    this.objData = {
      "name": name,
      "Email": email,
      "subject": subject,
      "message": message,
    }
    console.log(this.objData)
    this.http.post("https://formspree.io/f/mzbqyqrd", this.objData).subscribe({
      next: (data:any) => {
        console.log(data)
        alert("Form Submitted successfully...")
        // this.route.navigate(data.next);
      },
      error:(err:any)=>{console.log(err)}
   });

    }


}
