import {Component} from "@angular/core";
import {TranslateService, LangChangeEvent} from "ng2-translate";


@Component({
  moduleId: module.id,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']

})

export class AppComponent {

  private lang: string ="";

  constructor(public translate: TranslateService) {
    translate.setDefaultLang('en');

  }

  ngOnInit() {
  this.lang = localStorage.getItem("language");
    if(this.lang!== null){
      this.translate.use(this.lang);
    }
  }



}
