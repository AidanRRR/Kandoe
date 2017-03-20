import {browser, element, by, ExpectedConditions,  $} from "protractor";


describe('User should not be logged in', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/login');
  });

  it('User should go to login page', function () {
    expect(element(by.id('loginForm')).isPresent()).toBe(true);
  });
});
describe('Testing invites page user not logged in', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/invites');
  });

  it('User should go to login page', function () {
    expect(element(by.id('loginForm')).isPresent()).toBe(true);
  });
});

describe('Testing invites page user logged in', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/invites');
  });

  it('A user should login with correct credentials', function () {
    browser.driver.findElement(by.id('username')).sendKeys('vdvx');
    browser.driver.findElement(by.id('password')).sendKeys('ww');
    browser.driver.findElement(by.id('loginBtn')).submit();

    let logo = $('#kandoeLogo');
    expect(logo.isPresent()).toBeTruthy();
  });

  it('User should go to invites page', function () {
    expect(element(by.id('invitesPage')).isPresent()).toBe(true);
  });
});
