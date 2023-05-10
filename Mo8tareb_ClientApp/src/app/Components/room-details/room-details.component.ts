import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { AccountApiService } from 'src/app/Services/account-api.service';
import { Mo8tarebGBTService } from 'src/app/Services/mo8tareb-gbt.service';
import { RoomServiceService } from 'src/app/Services/room-service.service';
import { UserControllerServiceService } from 'src/app/Services/user-controller-service.service';
import { ReviewService } from 'src/app/Services/review-service.service';

@Component({
  selector: 'app-room-details',
  templateUrl: './room-details.component.html',
  styleUrls: ['./room-details.component.css']
})
export class RoomDetailsComponent {
  ID: any;
  room: any;




  constructor(myActivated: ActivatedRoute, private myService: RoomServiceService, private sanitizer: DomSanitizer,
    private AccountService:AccountApiService,private renderer: Renderer2, private AI: Mo8tarebGBTService, private account: AccountApiService
    ,private user:UserControllerServiceService, private reviewService:ReviewService) {
    this.ID = myActivated.snapshot.params["id"];
  }
  ngOnInit(): void {
    this.getUserName();

    this.myService.getRoomById(this.ID).subscribe({
      next:(data)=>{
        console.log(data)
        this.room = data;

        this.room.images = this.room.images.map((image:any) =>({
          id: image.id,
          imageUrl: this.sanitizer.bypassSecurityTrustUrl('data:image/png;base64,' + image.imageUrl)
          }))

        this.reviewService.GetAllReview(this.ID).then(res => {
          return res.json()
        })
          .then(data => {
            data.forEach((element: any) => {
              this.appendMessage(this.PERSON_NAME, this.PERSON_IMG, "left", element.comment,element.rating);

            });
          console.log(data)
          }).catch(err => {
            console.log(err)

        })
      },

      error:(err)=>{console.log(err)},
    })
  }




  @ViewChild('msgerForm') msgerForm!: ElementRef<HTMLFormElement>;
  @ViewChild('msgerInput') msgerInput!: ElementRef<HTMLInputElement>;
  @ViewChild('rangeInput') rangeInput!: ElementRef<HTMLInputElement>;
  @ViewChild('msgerChat') msgerChat!: ElementRef<HTMLBodyElement> ;



  private BOT_IMG = "https://cdn-icons-png.flaticon.com/512/1129/1129196.png";
  private PERSON_IMG = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQCRQLjKEmvEsX-E2U8m9r48UWRLLgl3iun4HV1W-EJnQZ6fvi35BH_afEMmR0IeCKXMUI&usqp=CAU";
  private BOT_NAME = "Mo8tareb GBT";

  private PERSON_NAME = "user";



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



  private appendMessage(name: string, img: string, side: string, text: string,rate:any) {

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
const msgInfoTimeText = this.renderer.createText(`${rate}/10`);
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


  // private botResponse(query: any) {

  //   this.AI.SendQuery(query)
  //     .then((response: { result: any; }) => {
  //       return response.result;
  //     })
  //     .then(data => {
  //       console.log(data);
  //       this.appendMessage(this.BOT_NAME, this.BOT_IMG, "left", data);
  //     })
  //     .catch(error => {
  //       console.error(error);
  //       this.appendMessage(
  //         this.BOT_NAME,
  //         this.BOT_IMG,
  //         "left",
  //         "My Owner Has Deactivated ME For Now, please wait until i come back soon."
  //       );
  //     });
  // }



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


  payload: any;

  sendMessage() {
    const msgText = this?.msgerInput?.nativeElement.value;
    if (!msgText) return;


    if (this.msgerInput?.nativeElement) {
      this.msgerInput.nativeElement.value = '';
    }
  }
  submitReview( ) {

    const msgText = this?.msgerInput?.nativeElement.value;
    const rangeText = this?.rangeInput?.nativeElement.value;
    if (!msgText) return;
    if (!rangeText || (+rangeText<0) ||(+rangeText>10) ) return;

    this.payload = {
        "rating": +rangeText,
        "userEmail": this.account.GetEmail(),
        "comments": msgText,
        "roomId":+this.ID
    };

    this.reviewService.submitReview(this.payload)
    .then(response => {
      return response;
    })
    .then(data => {
      console.log(data);
      this.appendMessage(this.PERSON_NAME, this.PERSON_IMG, "left",msgText,`${+rangeText}`);


      // console.log('Review submitted successfully');
      if (this.msgerInput?.nativeElement) {
        this.msgerInput.nativeElement.value = '';
        this.rangeInput.nativeElement.value = '';
      }
    }).catch(error => {
        console.log('Error submitting review: ', error);
      });

      this.reviewService.GetAllReview(this.ID)
      .then(response => {
        return response;
      })
      .then(data => {
        console.log(data);
        // this.appendMessage(this.getUserName(), this.BOT_IMG, "left", data);
        // console.log('Review submitted successfully');
        if (this.msgerInput?.nativeElement) {
          this.msgerInput.nativeElement.value = '';
          this.rangeInput.nativeElement.value = '';
        }
      }).catch(error => {
          console.log('Error submitting review: ', error);
        });

  }




}
