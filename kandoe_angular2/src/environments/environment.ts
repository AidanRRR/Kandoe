// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `angular-cli.json`.

export const environment = {
  production: false,
  ignoreAuth: false,
  signalUrl: 'http://146.185.172.29:5050/signalr',
  api: 'http://146.185.172.29:5010/api',
  themeAPI: 'http://146.185.172.29:5040/api',
  sessionAPI: 'http://146.185.172.29:5050/api',
  authAPI: 'http://146.185.172.29:5020/api',
  userAPI: 'http://146.185.172.29:5030/api',
  imageAPI: 'http://146.185.172.29:5060/api'
};
