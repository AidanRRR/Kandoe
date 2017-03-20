import {Injectable} from "@angular/core";
import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {Card, CardReaction} from "../domain/card";
import {Observable} from "rxjs";
import {environment} from "../environments/environment";


const API = environment.api;

@Injectable()
export class CardService {

  private options;

  constructor(private http: Http) {
    let headers = new Headers({'Content-Type': 'application/json'});
    this.options = new RequestOptions({headers: headers});
  }

  create(card: Card) {
    return this.http.post(API + '/card/addcard', {
      "imageUrl": card.imageUrl,
      "text": card.text,
      "themeId": card.themeId
    }, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  addCards(cards: Card[]) {
    let datastring = {};
    datastring['cards'] = [];
    for (let i = 0; i < cards.length; i++){
      cards[i].reactions = [];
      datastring['cards'].push(cards[i]);
    }

    return this.http.post(API + '/card/addCards', JSON.stringify(datastring), this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  getCard(id: string){
    return this.http.get(API + '/Card/'+id, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  addReaction(reaction: CardReaction){
    return this.http.post(API + '/card/addreaction', JSON.stringify(reaction), this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  extractData(res: Response) {
    let body = res.json();
    if (body.hasErrors) {
      return this.handleError("");
    } else return body.data;
  }

  handleError(error: Response | any) {
    // In a real world app, we might use a remote logging infrastructure
    let errMsg: string;
    if (error instanceof Response) {
      const body = error.json() || '';
      const err = body.error || JSON.stringify(body);
      errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    return Observable.throw(errMsg);
  }

}
