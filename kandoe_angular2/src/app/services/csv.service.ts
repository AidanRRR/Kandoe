import {Injectable} from "@angular/core";
import {Card} from "../domain/card";

@Injectable()
export class CsvService {

  constructor() {
  }

  read(event) {
    let cardsCSV = [];
    let file = event.target;
    let reader = new FileReader();

    reader.onloadend = (e) => {
      let csvData = reader.result;
      let allTextLines = csvData.split(/\r\n|\n/);
      let header = allTextLines[0].split(';');
      if (header[0] === "Omschrijving" && header[1] === "Url") {
        for (let i = 1; i < allTextLines.length; i++) {
          let data = allTextLines[i].split(';');

          let newCardCSV = {
            text: data[0],
            imageUrl: data[1]
          };

          cardsCSV.push(newCardCSV);
        }
      }
    };
    reader.readAsText(file.files[0]);
    return cardsCSV;
  }

  write(cards: Card[]) {
    let header = 'Omschrijving;Url';
    let row = '';
    let string = header + '\r\n';
    for (let i = 0; i < cards.length; i++) {
      row = cards[i].text + ";" + cards[i].imageUrl;
      string += row + '\r\n';
    }

    let a = document.createElement("a");
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    let blob = new Blob([string], {type: 'text/csv'});
    a.href = window.URL.createObjectURL(blob);
    a.download = 'cards:' + new Date().toISOString() + '.csv';
    a.click();
  }

}
