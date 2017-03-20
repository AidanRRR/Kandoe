import {Injectable} from "@angular/core";
import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {Theme} from "../domain/theme";
import {Observable} from "rxjs";
import {Push} from "ionic-native";
import {Platform, AlertController} from "ionic-angular";


@Injectable()
export class PushService {
  private options;

  constructor(private http: Http, public platform: Platform, public alertCtrl: AlertController) {
    let headers = new Headers({'Content-Type': 'application/json'});
    this.options = new RequestOptions({headers: headers});
  }

  initPushNotification(){
    console.log('initpush');
    if (!this.platform.is('cordova')) {
      console.warn("Push notifications not initialized. Cordova is not available - Run in physical device");
      return;
    }
    let push = Push.init({
      android: {
        senderID: "545732450401"
      },
      ios: {
        alert: "true",
        badge: false,
        sound: "true"
      },
      windows: {}
    });

    push.on('registration', (data) => {
      console.log("device token ->", data.registrationId);
      this.sendDeviceId(data.registrationId).subscribe(
        res =>  console.log(res),
        error => console.log(error)
      );
      //TODO - send device token to server

    });
    push.on('notification', (data) => {
      console.log('message', data.message);
      let self = this;
      //if user using app and push notification comes
      if (data.additionalData.foreground) {
        // if application open, show popup
        let confirmAlert = this.alertCtrl.create({
          title: 'New Notification',
          message: data.message,
          buttons: [{
            text: 'Ignore',
            role: 'cancel'
          }, {
            text: 'View',
            handler: () => {
              //TODO: Your logic here
             console.log(data.message);
            }
          }]
        });
        confirmAlert.present();
      } else {
        //if user NOT using app and push notification comes
        //TODO: Your logic on click of push notification directly
        // self.nav.push(DetailsPage, {message: data.message});
        console.log("Push notification clicked");
      }
    });
    push.on('error', (e) => {
      console.log(e.message);
    });
  }

  sendDeviceId(deviceId : any) {
    let currUser = JSON.parse(localStorage.getItem('username')).decoded.name;
    return this.http.post('http://52.23.212.7:8080/notificationservice-0.0.1-SNAPSHOT/user/add',{"userId":currUser,"deviceId":deviceId}, this.options)
      .map(this.handleRequest)
      .catch(this.handleError);
  }

  private handleRequest(res: Response) {
    let body = res.json();
    return body.data || {};
  }

  private handleError(error: Response | any) {
    // In a real world app, we might use a remote logging infrastructure
    let errMsg: string;
    if (error instanceof Response) {
      const body = error.json() || '';
      const err = body.error || JSON.stringify(body);
      errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    console.error(errMsg);
    return Observable.throw(errMsg);
  }


}
