import {browser, by, $, element} from "protractor";
describe('Testing register', function () {

  beforeEach(function () {
    browser.get('http://localhost:4200/register');
  });

  it('register form should exist', function () {
    expect(browser.getTitle()).toEqual('KanDoe');
    expect(element(by.className('logo')).isPresent()).toBe(true);
    expect(element(by.id('registerForm')).isPresent()).toBe(true);
  });

  it('Register with one or more fields not filled in should fail', function () {
    let error = $('.help-block');

    browser.driver.findElement(by.id('username')).sendKeys('');
    browser.driver.findElement(by.id('password')).sendKeys('testPW');
    browser.driver.findElement(by.id('email')).sendKeys('test.email@email.com');
    browser.driver.findElement(by.id('registerBtn')).submit();

    expect(error.isPresent()).toBeTruthy();

    browser.driver.findElement(by.id('username')).sendKeys('Test');
    browser.driver.findElement(by.id('password')).sendKeys('');
    browser.driver.findElement(by.id('email')).sendKeys('test.email@email.com');
    browser.driver.findElement(by.id('registerBtn')).submit();

    expect(error.isPresent()).toBeTruthy();

    browser.driver.findElement(by.id('username')).sendKeys('Test');
    browser.driver.findElement(by.id('password')).sendKeys('testpw');
    browser.driver.findElement(by.id('email')).sendKeys('');
    browser.driver.findElement(by.id('registerBtn')).submit();

    expect(error.isPresent()).toBeTruthy();
  });

  it('Register with wrong email format should fail', function () {
    let error = $('.help-block');

    browser.driver.findElement(by.id('username')).sendKeys('test');
    browser.driver.findElement(by.id('password')).sendKeys('testPW');
    browser.driver.findElement(by.id('email')).sendKeys('test.email');
    browser.driver.findElement(by.id('registerBtn')).submit();

    expect(error.isPresent()).toBeTruthy();
  });

  it('Pressing on annuleer button should bring user to login page', function () {
    browser.driver.findElement(by.id('cancelBtn')).click();

    let loginPage = $('#loginForm');
    expect(loginPage.isPresent()).toBeTruthy();
  });


  it('A user should register and redirect to loginpage', function () {
    browser.driver.findElement(by.id('username')).sendKeys('testRegisterUser');
    browser.driver.findElement(by.id('password')).sendKeys('testPW');
    browser.driver.findElement(by.id('email')).sendKeys('test.email@email.com');
    browser.driver.findElement(by.id('registerBtn')).submit();

    var loginForm = $('#loginForm');
    expect(loginForm.isPresent()).toBeTruthy();
  });

});
