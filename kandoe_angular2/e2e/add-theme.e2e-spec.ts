import {browser, element, by, ExpectedConditions,  $} from "protractor";


describe('User should not be logged in', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/login');
  });

  it('User should go to login page', function () {
    expect(element(by.id('loginForm')).isPresent()).toBe(true);
  });
});

describe('Testing Add Theme', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/themes');
  });

  it('A user should login with correct credentials', function () {
    browser.driver.findElement(by.id('username')).sendKeys('vdvx');
    browser.driver.findElement(by.id('password')).sendKeys('ww');
    browser.driver.findElement(by.id('loginBtn')).submit();

    let logo = $('#kandoeLogo');
    expect(logo.isPresent()).toBeTruthy();
  });

  it('An add theme button should exist', function () {
    expect(element(by.className('page-header')).isPresent()).toBe(true);
  });

  it('Filling in the form should add a theme', function () {
    browser.driver.findElement(by.id('page-header-btn')).click();

    var form = element(by.className('modal-title'));
    var EC = ExpectedConditions;
    browser.wait(EC.visibilityOf(form), 10000);
    expect(form.isDisplayed()).toBeTruthy();

    browser.driver.findElement(by.id('name')).sendKeys('zwembad');
    browser.driver.findElement(by.id('description')).sendKeys('thema over zwembad');
    browser.driver.findElement(by.id('newTag')).sendKeys('zwemmen, openbaar, instelling, gemeente, prive');
    browser.driver.findElement(by.className('btn btn-primary')).click();

    expect(element(by.className('toast toast-success')).isPresent()).toBe(true);

  });

});
