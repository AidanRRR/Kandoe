import {Component, OnInit, ViewChild} from "@angular/core";
import {AuthService} from "../../../services/auth.service";
import {UserService} from "../../../services/user.service";
import {Router, ActivatedRoute} from "@angular/router";
import {TranslateService} from "ng2-translate";
import {PushNotificationComponent} from "ng2-notifications/src/app/components/notification.component";
let SockJS = require('sockjs-client');
let Stomp = require('stompjs');

@Component({
  selector: 'header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  private username: string;
  private search: string = "";
  private loggedIn: boolean = false;
  isConnected: boolean = false;
  private ref: any;
  @ViewChild('myNotification') notif;

  public notification: any = {
    show: false,
    title: 'Kandoe Notification',
    body: '',
    icon: 'src/assets/images/kandoe.png'
  };

  constructor(private auth: AuthService, private userService: UserService, private router: Router, public translate: TranslateService) {
    this.username = this.userService.getCurrentUsername();
    this.loggedIn = !!this.username;
  }

  ngOnInit() {
    if (this.loggedIn){
      this.connect();
    }
  }

  private onSearchClick() {
    this.router.navigate(['/publicThemes', this.search]);
  }

  logout() {
    this.auth.logout();
  }

  changeLanguage(language: string) {
    this.translate.use(language);
    localStorage.setItem("language",language);
    console.log(this.translate.currentLang);
  }



  public connect() {
    let socket = new SockJS('http://52.23.212.7:8080/notificationservice-0.0.1-SNAPSHOT/ip-notification');
    let stompclient = Stomp.over(socket);
    let self = this;
    if (!this.isConnected) {
      stompclient.connect({}, function (frame) {
        console.log('Connected: ' + frame);
        self.subscribe(stompclient);
      }, function (err) {
        console.log('err', err);
      }).bind(this);
    } this.isConnected = true;
  }


  subscribe = (stompclient) =>{
    console.log("subscribed");
    let self = this;
    let currUser = JSON.parse(localStorage.getItem('username'));
    console.log(currUser);
    stompclient.subscribe('/ip-user/'+currUser+'/new-notification', function (message) {
      console.log("received notification");
      console.log(JSON.parse(message.body).content);
      self.fireNotification(JSON.parse(message.body).content);
    });
  };



  fireNotification(message){
    this.notification.body = message;
    this.notif.show();
  }

}
