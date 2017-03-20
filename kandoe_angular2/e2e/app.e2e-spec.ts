import {browser, by, $, element} from "protractor";

describe('User should not be logged in', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/login');
  });

  it('User should go to login page', function () {
    expect(element(by.id('loginForm')).isPresent()).toBe(true);
  });
});

describe('Testing WebApp landing page with no user logged in', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/');
  });

  it('key-elements should exist', function () {
    expect(browser.getTitle()).toEqual('KanDoe');
  });

  it('User should go to about if not logged in', function () {
    let about = $('#AboutPage');
    expect(about.isPresent()).toBeTruthy();
  });
});

describe('Testing WebApp landing page with user logged in', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/login');
    let loginPage = $('#loginForm');
    expect(loginPage.isPresent()).toBeTruthy();
    browser.driver.findElement(by.id('username')).sendKeys('vdvx');
    browser.driver.findElement(by.id('password')).sendKeys('ww');
    browser.driver.findElement(by.id('loginBtn')).submit();
  });

  it('key-elements should exist', function () {
    expect(browser.getTitle()).toEqual('KanDoe');

  });

  it('User should go to about if logged in', function () {
    let logo = $('#kandoeLogo');
    expect(logo.isPresent()).toBeTruthy();

    let about = $('#AboutPage');
    expect(about.isPresent()).toBeTruthy();
  });



});

