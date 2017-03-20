import {Injectable} from "@angular/core";
import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {Theme} from "../domain/theme";
import {Observable} from "rxjs";
import {environment} from "../environments/environment";


const API = environment.api;

@Injectable()
export class ThemeService {
  private options;

  constructor(private http: Http) {
    let headers = new Headers({'Content-Type': 'application/json'});
    this.options = new RequestOptions({headers: headers});
  }

  create(theme: Theme) {
    theme.organizers = [];
    theme.organizers.push(theme.username);

    return this.http.post(API + '/theme/addtheme', JSON.stringify(theme), this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  getThemesByUser(name: string) {
    return this.http.get(API + '/theme/getthemesbyuser/' + name, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  getPublicThemes() {
    return this.http.get(API + '/theme/publicthemes', this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  getTheme(id: string) {
    return this.http.get(API + '/theme/gettheme/' + id, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  updateTheme(theme: Theme) {
    return this.http.post(API + '/theme/updatetheme', {
      "themeId": theme.themeId,
      "name": theme.name,
      "description": theme.description,
      "organizers": theme.organizers,
      "isPublic": theme.isPublic,
      "tags": theme.tags,
    }, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  removeTheme(id: string) {
    return this.http.put(API + '/theme/disabletheme/' + id, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  private extractData(res: Response) {
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
