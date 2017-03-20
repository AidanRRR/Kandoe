import {Injectable} from "@angular/core";
import {RequestOptions, Headers, Http, Response} from "@angular/http";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";

const API = environment.imageAPI;
declare const jQuery: any;

@Injectable()
export class ImageService {
  private options;

  constructor(private http: Http) {
    let headers = new Headers();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    this.options = new RequestOptions({headers: headers});
  }

  postImage(file) {
    let formData = new FormData();
    formData.append("file", file);
    
    let headers = new Headers();
    headers.append('Accept', 'application/json');
    let options = new RequestOptions({ headers: headers });
    
    return this.http.post(API + '/image/PostImage', formData, options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  private extractData(res: Response) {    
    let body = res.json();
    return body || {};
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
