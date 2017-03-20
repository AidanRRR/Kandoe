import {Component, OnInit} from "@angular/core";
import {Theme} from "../../domain/theme";
import {ThemeService} from "../../services/theme.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-public-themes',
  templateUrl: './public-themes.component.html',
  styleUrls: ['./public-themes.component.css']
})
export class PublicThemesComponent implements OnInit {

  private filterThemes: Theme[] = [];

  constructor(private themeService: ThemeService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    if (this.route.params != null) {
      this.route.params.subscribe(params => this.initialize(params['filter']));
    }
  }

  private filterData(searchFilter, themes: Theme[]) {
    if (searchFilter && searchFilter.trim() != '') {
      this.filterThemes = themes.filter((item) => {
        if (item.name.toLowerCase().indexOf(searchFilter.toLowerCase()) > -1) return true;
        else if (item.description.toLowerCase().indexOf(searchFilter.toLowerCase()) > -1) return true;
        else if (item.tags.toString().toLowerCase().indexOf(searchFilter.toLowerCase()) > -1) return true;
      });
    } else {
      this.filterThemes = themes;
    }
  }

  private initialize(param: string) {
    this.themeService.getPublicThemes().subscribe(
      data => {
        this.filterData(param, data);
        console.log("get all themes succes");
      },
      error => {
        console.log("get  all themes failed");
      });
  }
}
