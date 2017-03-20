import {Component, OnInit, Input} from "@angular/core";
import {Theme} from "../../../domain/theme";
import {Router} from "@angular/router";

@Component({
  selector: 'theme-card',
  templateUrl: './theme-card.component.html',
  styleUrls: ['./theme-card.component.css']
})
export class ThemeCardComponent implements OnInit {

  constructor(private router: Router) {
  }

  ngOnInit() {
  }

  @Input()
  public theme: Theme;


  private onEditClick(id: string) {
    this.router.navigate(['/theme', id]);
  }

}
