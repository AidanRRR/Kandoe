import {Component, OnInit} from "@angular/core";
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";
import {User} from "../../domain/user";

declare const toastr: any;

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  private user: User;

  constructor(private userService: UserService, private router: Router) {
  }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij ophalen user');
          console.log(data.errorMessages[0]);
        }
        else {
          this.user = data;
        }
      },
      error => {
        toastr.error('Fout bij ophalen user');
        console.log(error);
      });

  }

  public update(): void {
    this.userService.updateUser(this.user).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij updaten user');
          console.log(data.errorMessages[0]);
        }
        else {
          toastr.success('Profiel updaten gelukt');
          this.user = data;
        }
      },
      error => {
        toastr.error('Fout bij updaten user');
        console.log(error);
      });
  }

}
