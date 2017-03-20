import {browser, element, by, ExpectedConditions,  $} from "protractor";

describe('Testing Get Public Themes', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/');
  });

  it('Landing page', function () {
    let logo = $('#kandoeLogo');
    expect(logo.isPresent()).toBeTruthy();
  });

  it('search bar should appear', function () {
    expect(element(by.id('search')).isPresent()).toBe(true);
  });

  it('A user should be able to search', function () {
    expect(element(by.id('search')).isPresent()).toBe(true);
    browser.driver.findElement(by.id('search')).sendKeys('teest');
    browser.driver.findElement(by.id('searchButton')).click();

    let publicThemesPage = $('#publicThemesPage');
    expect(publicThemesPage.isPresent()).toBeTruthy();
  });

});
