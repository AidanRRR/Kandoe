import {Component, OnInit} from "@angular/core";
import {Theme} from "../../domain/theme";
import {ThemeService} from "../../services/theme.service";
import {UserService} from "../../services/user.service";
import {WebSocketService} from "../../services/websocket.service";


declare const toastr: any;

@Component({
  selector: 'themes',
  templateUrl: './themes.component.html',
  styleUrls: ['./themes.component.css']
})
export class ThemesComponent implements OnInit {

  private newTheme: any = {};
  private newTag: string;
  private themes: Theme[];
  private name: string;

  constructor(private themeService: ThemeService, private userService: UserService) {
    this.name = this.userService.getCurrentUsername();
  }

  ngOnInit() {
    this.themeService.getThemesByUser(this.name).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij ophalen thema\'s');
          console.log(data.errorMessages[0]);
        }
        else {
          this.themes = data;
        }
      },
      error => {
        console.log(error);
        toastr.error('Fout bij ophalen thema\'s');
      });
  }

  addTheme() {
    this.newTheme.username = this.name;
    this.themeService.create(this.newTheme).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Thema toevoegen mislukt');
          console.log(data.errorMessages[0]);
        }
        else {
          let newTheme:Theme = data;
          toastr.success("Thema toevoegen gelukt!");
          this.themes.push(newTheme);
        }
      },
      error => {
        console.log(error);
        toastr.error("Thema toevoegen mislukt!");
      });
  }

  addNewTag() {
    if (this.newTheme.tags == null) this.newTheme.tags = [];
    this.newTheme.tags.push(this.newTag);
  }

  removeTag(tag: string) {
    this.newTheme.tags = this.newTheme.tags.filter(item => item !== tag);
  }
}
