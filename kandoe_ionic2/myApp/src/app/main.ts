import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app.module';
import {environment} from "../environments/environment";
import {enableProdMode} from "@angular/core";

// Normale Angular2 initialisatie logica
function loadApp() {
  if (environment.production) {
    enableProdMode();
  }
  platformBrowserDynamic().bootstrapModule(AppModule);
}
// Laden van signalR hub JS voor we de angular2 app initialiseren
// Afhankelijk van dev/production build andere URL
declare var jQuery: any;
jQuery.getScript(environment.signalUrl + '/hubs', () => {
  loadApp();
})
  .fail(() => {
    console.error('Laden SignalR hub JS mislukt');
    loadApp();
  });
