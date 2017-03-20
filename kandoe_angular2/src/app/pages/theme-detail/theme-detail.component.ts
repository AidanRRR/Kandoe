import {Component, OnInit} from "@angular/core";
import {Card} from "../../domain/card";
import {ThemeService} from "../../services/theme.service";
import {ActivatedRoute, Router} from "@angular/router";
import {CardService} from "../../services/card.service";
import {CsvService} from "../../services/csv.service";
import {SessionService} from "../../services/session.service";
import {Session} from "../../domain/session";
import {ImageService} from "../../services/image.service";
import {UserService} from "../../services/user.service";
import {Theme} from "../../domain/theme";

declare const toastr: any;

@Component({
  selector: 'app-theme-detail',
  templateUrl: './theme-detail.component.html',
  styleUrls: ['./theme-detail.component.css']
})

export class ThemeDetailComponent implements OnInit {

  private currentThemeId: string;
  private currentTheme: Theme;
  private isOrganizer = false;
  private cards: Card[];
  private sessions: Session[];

  private newTag: string = '';
  private newUser: string = '';

  private cloneThemeName: string = "";

  private newCard: any = {};
  private cardsCSV: any[] = [];

  private newSession: any = {};
  private types = ['Kansencirkel', 'Problemencirkel'];

  constructor(private themeService: ThemeService, private userService: UserService, private cardService: CardService, private imageService: ImageService, private csvService: CsvService, private sessionService: SessionService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit() {
    if (this.route.params != null) {
      this.route.params.subscribe(params => this.currentThemeId = params['id']);
    }

    let username = this.userService.getCurrentUsername();

    this.themeService.getTheme(this.currentThemeId).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij ophalen thema');
          console.log(data.errorMessages[0]);
        }
        else {
          this.currentTheme = data;
          this.cards = this.currentTheme.cards;
          if (this.currentTheme.organizers.indexOf(username) > 1 || this.currentTheme.username === username) this.isOrganizer = true;
        }
      },
      error => {
        console.log(error);
        toastr.error('Fout bij ophalen thema');
      });

    this.sessionService.getSessionsByTheme(this.currentThemeId).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij ophalen sessies');
          console.log(data.errorMessages[0]);
        }
        else {
          this.sessions = data;
        }
      },
      error => {
        console.log(error);
        toastr.error('Fout bij ophalen sessies');
      });
  }

  removeTheme() {
    this.themeService.removeTheme(this.currentThemeId).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij verwijderen thema');
          console.log(data.errorMessages[0]);
        }
        else {
          this.router.navigate(['/themes']);
          toastr.success('Thema is verwijderd');
        }
      },
      error => {
        toastr.error('Fout bij verwijderen thema');
        console.log(error);
      });
  }

  editTheme() {
    if (this.cards != null) this.currentTheme.cards = this.cards;
    this.themeService.updateTheme(this.currentTheme).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij updaten thema');
          console.log(data.errorMessages[0]);
        }
        else {
          this.currentTheme = data;
          toastr.success("Thema updaten gelukt!");
        }
      },
      error => {
        toastr.error("Thema updaten mislukt!");
        console.log(error);
      });
  }

  addNewTag() {
    if (this.currentTheme.tags == null) this.currentTheme.tags = [];
    this.currentTheme.tags.push(this.newTag);
  }

  removeTag(tag: string) {
    this.currentTheme.tags = this.currentTheme.tags.filter(item => item !== tag);
  }

  addNewUser() {
    if (this.currentTheme.organizers == null) this.currentTheme.organizers = [];
    this.currentTheme.organizers.push(this.newUser);
  }

  removeUser(user: string) {
    this.currentTheme.organizers = this.currentTheme.organizers.filter(item => item !== user);
  }


  cardClick(card: Card) {
    card.selected = !card.selected;
  }

  addCard() {
    let newCard: Card = this.newCard;
    newCard.themeId = this.currentThemeId;
    this.cardService.create(newCard).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij aanmaken kaart');
          console.log(data.errorMessages[0]);
        }
        else {
          this.cards.push(data);
          toastr.success("Kaart toevoegen gelukt!");
        }
      },
      error => {
        toastr.error('Kaart toevoegen mislukt');
        console.log(error);
      });
  }

  onChangeImg(event) {
    let file = event.target.files[0];
    this.imageService.postImage(file).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij aanmaken kaart');
          console.log(data.errorMessages[0]);
        }
        else {
          this.newCard.imageUrl = data.uri;
          toastr.success("Kaart toevoegen gelukt!");
        }
      },
      error => {
        toastr.error('Kaart toevoegen mislukt');
        console.log(error);
      });
  }

  onChange(event) {
    this.cardsCSV = this.csvService.read(event);
  }

  importCards() {
    let cards: Card[] = [];
    for (let i = 0; i < this.cardsCSV.length; i++) {
      let newCard: Card = this.cardsCSV[i];
      newCard.themeId = this.currentThemeId;
      if (newCard.text !== "") {
        cards.push(newCard);
      }
    }
    if (cards.length > 0) {
      this.cardService.addCards(cards).subscribe(
        data => {
          if (data.hasErrors) {
            toastr.error("Kaarten importeren mislukt");
            console.log(data.errorMessages[0]);
          }
          else {
            for (let i = 0; i < data.length; i++) this.cards.push(data[i]);
            toastr.success("Kaarten importeren gelukt");
          }
        },
        error => {
          console.log(error);
          toastr.error("Kaarten importeren mislukt");
        });
      this.cardsCSV = [];
    } else toastr.error("Kaarten importeren mislukt");
  }

  exportCards() {
    this.csvService.write(this.cards);
  }

  addSession() {
    this.newSession.themeId = this.currentThemeId;
    if (this.newSession.typeCircle === "Problemencirkel")
      this.newSession.circleType = 1;
    else
      this.newSession.circleType = 0;
    this.newSession.cards = this.cards.filter(c => c.selected);

    this.sessionService.create(this.newSession).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error("Sessie toevoegen mislukt - "+ data.errorMessages[0]);
          console.log(data.errorMessages[0]);
        }
        else {
          this.sessions.push(data.data);
          toastr.success("Sessie toevoegen gelukt!");
        }
      },
      error => {
        toastr.error("Sessie toevoegen mislukt!");
        console.error(error);
      });
  }

  cloneTheme() {
    let newTheme: Theme = this.currentTheme;
    newTheme.name = this.cloneThemeName;
    newTheme.themeId = "";
    this.themeService.create(newTheme).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error("Thema toevoegen mislukt!");
          console.log(data.errorMessages[0]);
        }
        else {
          toastr.success("Thema toevoegen gelukt!");
        }
      },
      error => {
        console.log(error);
        toastr.error("Thema toevoegen mislukt!");
      });
  }
}
