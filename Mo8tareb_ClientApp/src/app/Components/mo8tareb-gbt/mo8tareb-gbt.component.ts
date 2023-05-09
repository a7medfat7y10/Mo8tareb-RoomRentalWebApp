import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { Mo8tarebGBTService } from 'src/app/Services/mo8tareb-gbt.service';
import { UserControllerServiceService } from 'src/app/Services/user-controller-service.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-mo8tareb-gbt',
  templateUrl: './mo8tareb-gbt.component.html',
  styleUrls: ['./mo8tareb-gbt.component.css'],
  providers:[Mo8tarebGBTService]
})
export class Mo8tarebGBTComponent implements OnInit {

  @ViewChild('msgerForm') msgerForm!: ElementRef<HTMLFormElement>;
  @ViewChild('msgerInput') msgerInput!: ElementRef<HTMLInputElement>;
  @ViewChild('msgerChat') msgerChat!: ElementRef<HTMLBodyElement> ;

  private BOT_MSGS: string[] = [
    "Hi, how are you?",
    "Ohh... I can't understand what you trying to say. Sorry!",
    "I like to play games... But I don't know how to play!",
    "Sorry if my answers are not relevant. :))",
    "I feel sleepy! :("
  ];

  private BOT_IMG = "https://cdn-icons-png.flaticon.com/512/1129/1129196.png";
  private PERSON_IMG = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQCRQLjKEmvEsX-E2U8m9r48UWRLLgl3iun4HV1W-EJnQZ6fvi35BH_afEMmR0IeCKXMUI&usqp=CAU";
  private BOT_NAME = "Mo8tareb GBT";

  private PERSON_NAME = "user";

  constructor(private renderer: Renderer2, private AI: Mo8tarebGBTService, private account: AccountApiService
  ,private user:UserControllerServiceService,private translate: TranslateService) {
  }
  ngOnInit(): void {
    this.getUserName();
  }
  getUserName() {


    this.user.getUserByEmail().subscribe({
      next: (data: any) => {
        // console.log(data)
        this.PERSON_NAME = data.firstName + " " + data.lastName;
      },
      error: (err:any) => {

      }
    })
  }
  sendMessage() {
    const msgText = this?.msgerInput?.nativeElement.value;
    if (!msgText) return;

    this.appendMessage(this.PERSON_NAME, this.PERSON_IMG, "right", msgText);

    this.botResponse(msgText);
    if (this.msgerInput?.nativeElement) {
      this.msgerInput.nativeElement.value = '';
    }
  }

  private appendMessage(name: string, img: string, side: string, text: string) {

const msgDiv = this.renderer.createElement('div');
this.renderer.addClass(msgDiv, 'msg');
this.renderer.addClass(msgDiv, `${side}-msg`);

const msgImgDiv = this.renderer.createElement('div');
this.renderer.addClass(msgImgDiv, 'msg-img');
this.renderer.setStyle(msgImgDiv, 'background-image', `url(${img})`);

const msgBubbleDiv = this.renderer.createElement('div');
this.renderer.addClass(msgBubbleDiv, 'msg-bubble');

const msgInfoDiv = this.renderer.createElement('div');
this.renderer.addClass(msgInfoDiv, 'msg-info');

const msgInfoNameDiv = this.renderer.createElement('div');
this.renderer.addClass(msgInfoNameDiv, 'msg-info-name');
const msgInfoNameText = this.renderer.createText(name);
this.renderer.appendChild(msgInfoNameDiv, msgInfoNameText);

const msgInfoTimeDiv = this.renderer.createElement('div');
this.renderer.addClass(msgInfoTimeDiv, 'msg-info-time');
const msgInfoTimeText = this.renderer.createText(this.formatDate(new Date()));
this.renderer.appendChild(msgInfoTimeDiv, msgInfoTimeText);

const msgTextDiv = this.renderer.createElement('div');
this.renderer.addClass(msgTextDiv, 'msg-text');
const msgText = this.renderer.createText(text);
this.renderer.appendChild(msgTextDiv, msgText);

this.renderer.appendChild(msgInfoDiv, msgInfoNameDiv);
this.renderer.appendChild(msgInfoDiv, msgInfoTimeDiv);

this.renderer.appendChild(msgBubbleDiv, msgInfoDiv);
this.renderer.appendChild(msgBubbleDiv, msgTextDiv);

this.renderer.appendChild(msgDiv, msgImgDiv);
this.renderer.appendChild(msgDiv, msgBubbleDiv);

this.renderer.appendChild(this.msgerChat.nativeElement, msgDiv);

    if (this.msgerChat && this.msgerChat.nativeElement) {
      this.msgerChat.nativeElement.scrollTop += 500;
    }
  }

  private botResponse(query: any) {

    this.AI.SendQuery(query)
      .then(response => {
        return response.result;
      })
      .then(data => {
        console.log(data);
        this.appendMessage(this.BOT_NAME, this.BOT_IMG, "left", data);
      })
      .catch(error => {
        console.error(error);
        this.appendMessage(
          this.BOT_NAME,
          this.BOT_IMG,
          "left",
          "My Owner Has Deactivated ME For Now, please wait until i come back soon."
        );
      });
  }



  private get(selector: string, root = document) {
    return root.querySelector(selector);
  }

  private formatDate(date: Date) {
    const h = "0" + date.getHours();
    const m = "0" + date.getMinutes();
console.log(date.getHours())
console.log(date.getMinutes())
    return `${h.slice(-2)}:${m.slice(-2)}`;
  }

  private random(min: number, max: number) {
    return Math.floor(Math.random() * (max - min) + min);
  }

  isRtl(): boolean {
    const currentLang = this.translate.currentLang;
    return currentLang === 'ar';
  }
  
}
