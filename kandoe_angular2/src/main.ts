import "./polyfills.ts";
import {platformBrowserDynamic} from "@angular/platform-browser-dynamic";
import {enableProdMode} from "@angular/core";
import {environment} from "./environments/environment";
import {AppModule} from "./app/app.module";

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
