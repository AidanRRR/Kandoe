import { MockTranslatePipe } from './mock-translate.pipe';

describe('MockTranslatePipe', () => {
  it('create an instance', () => {
    const pipe = new MockTranslatePipe();
    expect(pipe).toBeTruthy();
  });
});
