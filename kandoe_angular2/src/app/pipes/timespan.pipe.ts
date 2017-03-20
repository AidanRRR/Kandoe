import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  name: 'timespan',
  pure: false
})
export class TimespanPipe implements PipeTransform {

  pad(x: number): string {
    return (x < 10) ? ('0' + x) : '' + x;
  }

  transform(value: any, args?: any): any {
    let x = Math.floor(value / 1000);
    if (x <= 0) {
      return '00:00';
    }

    let s = x % 60;
    let m = Math.floor(x / 60) % 60;
    let h = Math.floor(x / 60 / 60);

    if (h > 0) {
      return h + ':' + this.pad(m) + ':' + this.pad(s);
    }
    else {
      return this.pad(m) + ':' + this.pad(s);
    }
  }

}
