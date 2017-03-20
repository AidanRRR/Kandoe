//Dependencies
var express = require('express');
var app = express();
var bodyParse = require('body-parser');
var morgan = require('morgan');
var mongoose = require('mongoose');
var pwh = require('password-hash');
var jwt = require('jsonwebtoken');
var request = require('request');

//Imports
var config = require('./Config');
var User = require('./app/models/User');

//MongoDB
var port = 5020;
mongoose.connect(config.database);
app.set('superSecret',config.secret);

//MiddleWare
app.use(bodyParse.urlencoded({extended: false}));
app.use(bodyParse.json());
app.use(morgan('dev'));

var apiRoutes = express.Router();

app.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, x-acces-token");
  next();
});

apiRoutes.post('/register', function(req, response){
    var name = req.body.username;
    var password = req.body.password;
    var hashedpw = pwh.generate(password);
    var email = req.body.email;

    var requestData = "{\"userName\":\"" + name + "\",\"email\":\"" + email + "\"}";

    request({
        url: 'http://userservice:5030/api/Users/AddUser',
        method: 'POST',
        encoding: null,
        json: JSON.parse(requestData)
    }, function(err, res){
        if(!err && res.statusCode == 200){
            var user = new User({
                name: name,
                password: hashedpw,
                admin: true
            });

            user.save(function (err) {
                if(err) throw err;
                console.log('User Created');
                response.status(200).json({success: true})
            });
        }
        else{
            response.status(403).json({success: false})
        }

    });
});

apiRoutes.post('/login', function(req, res){
    console.log(req);
    User.findOne({
        name: req.body.username
    }, function(err, user){
        if(err) throw err;
        if(!user){
            res.status(403).json({ success: false, message: 'Authentication failed. User not found'});
        } else if(user){
            if(!pwh.verify(req.body.password, user.password)){
                res.status(403).json({ success: false, message: 'Authentication failed. Bad Password'});
            } else{
                var token = jwt.sign({name: user.name}, app.get('superSecret'), {
                    expiresIn: "1d"
                });

                res.status(200).json({
                    success: true,
                    message: 'Enjoy your token!',
                    token: token,
                    username: user.name
                });
            }
        }
    });
});

apiRoutes.post('/verify', function(req, res){
    var token = req.headers['x-acces-token'];

    if(token){
        jwt.verify(token, app.get('superSecret'), function(err, decoded){
            if(err){
                res.status(403).json({ success: false, message: 'Failed to authenticate token'});
            } else {
                res.status(200).json({ success: true, decoded})
            }
        });
    } else {
        res.status(403).json({
            success: false,
            message: 'No Token Provided'
        });
    }
});

app.use('/api/auth', apiRoutes);

app.listen(port);
console.log('AuthService Listening at port ' + port);
