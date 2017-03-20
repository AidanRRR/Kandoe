const express = require('express');
const config = require('./gateway.json')
const httpProxy = require('http-proxy');
//const cors = require('cors');
//const bodyparser = require('body-parser');

let app = express();
//app.use(cors());
let proxy = httpProxy.createProxyServer();

app.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, x-acces-token");
  res.header("Content-Type", "application/json");
  next();
});

config.services.forEach(svc => {
	app.all(svc.match, (req, res) => {
		console.log('Redirect ' + req.originalUrl + ' --> ' + svc.url);
		proxy.web(req, res, {target: svc.url});
	});
});

console.log('Gateway luistert op poort: ' + config.port);
app.listen(config.port);

