import {browser, element, by, $} from "protractor";
describe('Testing login', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/login');
  });

  it('login form should exist', function () {
    expect(browser.getTitle()).toEqual('KanDoe');
    expect(element(by.className('logo')).isPresent()).toBe(true);
    expect(element(by.id('loginForm')).isPresent()).toBe(true);
  });

  it('Login with empty username and password should fail', function () {
    browser.driver.findElement(by.id('username')).sendKeys('');
    browser.driver.findElement(by.id('password')).sendKeys('');
    browser.driver.findElement(by.id('loginBtn')).submit();

    let error = $('.help-block');
    expect(error.isPresent()).toBeTruthy();
  });

  it('Login with no username should fail', function () {
    browser.driver.findElement(by.id('username')).sendKeys('');
    browser.driver.findElement(by.id('password')).sendKeys('rwherhehehe5hge54h54e');
    browser.driver.findElement(by.id('loginBtn')).submit();

    let error = $('.help-block');
    expect(error.isPresent()).toBeTruthy();
  });

  it('Login with no password should fail', function () {
    browser.driver.findElement(by.id('username')).sendKeys('rwherhehehe5hge54h54e');
    browser.driver.findElement(by.id('password')).sendKeys('');
    browser.driver.findElement(by.id('loginBtn')).submit();

    let error = $('.help-block');
    expect(error.isPresent()).toBeTruthy();
  });

  it('Pressing on register button should bring user to register page', function () {
    browser.driver.findElement(by.id('registerBtn')).click();

    let registerPage = $('#registerForm');
    expect(registerPage.isPresent()).toBeTruthy();
  });

  it('A user should login with correct credentials', function () {
    browser.driver.findElement(by.id('username')).sendKeys('vdvx');
    browser.driver.findElement(by.id('password')).sendKeys('ww');
    browser.driver.findElement(by.id('loginBtn')).submit();

    let logo = $('#kandoeLogo');
    expect(logo.isPresent()).toBeTruthy();
  });

  it('Login with wrong credentials should fail', function () {
    browser.driver.findElement(by.id('username')).sendKeys('rwherhehehe5hge54h54e');
    browser.driver.findElement(by.id('password')).sendKeys('rwherhehehe5hge54h54e');
    browser.driver.findElement(by.id('loginBtn')).submit();

    expect(element(by.className('toast toast-error')).isPresent()).toBe(true);
  });
});
