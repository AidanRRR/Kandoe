var express = require('express');
var serveStatic = require('serve-static');
var path = require('path');

const PORT = 3000;

var app = express();

app.use(serveStatic('.', {'index': ['index.html']}));

app.get('/*', function (req, res) {
  res.sendFile(path.join(__dirname, 'index.html'))
});

console.log('Applicatie luistert op poort ' + PORT);
app.listen(PORT);
