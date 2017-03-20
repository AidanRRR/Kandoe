import {browser, element, by, ExpectedConditions,  $} from "protractor";

describe('Testing not found page', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/shdwreherhwgwhweher');
  });

  it('User should go to not-found page', function () {
    expect(element(by.id('notFoundPage')).isPresent()).toBe(true);
  });

  it('User should go back to homepage/about when clicking home button', function () {
    browser.driver.findElement(by.id('homeButton')).click();

    let about = $('#AboutPage');
    expect(about.isPresent()).toBeTruthy();
  });
});
