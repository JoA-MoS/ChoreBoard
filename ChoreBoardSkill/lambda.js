// alexa-cookbook sample code

// There are three sections, Text Strings, Skill Code, and Helper Function(s).
// You can copy and paste the entire file contents as the code for a new Lambda function,
//  or copy & paste section #3, the helper function, to the bottom of your existing Lambda code.


// 1. Text strings =====================================================================================================
//    Modify these strings and messages to change the behavior of your Lambda function


// 2. Skill Code =======================================================================================================


var Alexa = require('alexa-sdk');

exports.handler = function (event, context, callback) {
    var alexa = Alexa.handler(event, context);

    // alexa.appId = 'amzn1.echo-sdk-ams.app.1234';
    // alexa.dynamoDBTableName = 'YourTableName'; // creates new table for session.attributes

    alexa.registerHandlers(handlers);
    alexa.execute();
};

var handlers = {
    'LaunchRequest': function () {
        this.emit('GetChores');
    },

    'GetChores': function () {
        httpGet(myRequest, (myResult) => {
            console.log("sent     : " + myRequest);
            console.log("received : " + myResult);
            this.emit(':tell', 'Your chore today is ' + myResult);
        }
        );

    },
    'CreateChore': function () {
        httpGet(myRequest, (myResult) => {
            console.log("sent     : " + myRequest);
            console.log("received : " + myResult);
            this.emit(':tell', 'Your chore today is ' + myResult);
        }
        );
    },

    'MyIntent': function () {

        var pop = 0;
        var myRequest = 'Virginia';

        httpPost(myRequest, myResult => {
            console.log("sent     : " + myRequest);
            console.log("received : " + myResult);

            this.emit(':tell', 'The population of ' + myRequest + ' is ' + myResult);

        }
        );

    }
};


//    END of Intent Handlers {} ========================================================================================
// 3. Helper Function  =================================================================================================


var http = require('http');
// http is a default part of Node.JS.  Read the developer doc:  http://nodejs.org/api/http.html
// try other APIs such as the current bitcoin price : http://btc-e.com/api/2/btc_usd/ticker  returns ticker.last

function httpGet(myData, callback) {

    // GET is a web service request that is fully defined by a URL string
    // Try GET in your browser:
    // http://cp6gckjt97.execute-api.us-east-1.amazonaws.com/prod/stateresource?usstate=New%20Jersey


    // Update these options with the details of the web service you would like to call
    var options = {
        host: '34.209.13.220',
        port: 80,
        path: '/api/boards/1/chores/1',
        method: 'GET',

        // if x509 certs are required:
        // key: fs.readFileSync('certs/my-key.pem'),
        // cert: fs.readFileSync('certs/my-cert.pem')
    };

    var req = http.request(options, res => {
        res.setEncoding('utf8');
        var returnData = "";

        res.on('data', chunk => {
            returnData = returnData + chunk;
        });

        res.on('end', () => {
            // we have now received the raw return data in the returnData variable.
            // We can see it in the log output via:
            // console.log(JSON.stringify(returnData))
            // we may need to parse through it to extract the needed data

            var pop = JSON.parse(returnData).name;

            callback(pop);  // this will execute whatever function the caller defined, with one argument

        });

    });
    req.end();

}


function httpPost(myData, callback) {

    // GET is a web service request that is fully defined by a URL string
    // Try GET in your browser:
    // http://cp6gckjt97.execute-api.us-east-1.amazonaws.com/prod/stateresource?usstate=New%20Jersey


    var post_data = { "usstate": myData };

    var post_options = {
        host: '34.209.13.220',
        port: 80,
        path: '/api/boards/1/chores/',
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Content-Length': Buffer.byteLength(JSON.stringify(post_data))
        }
    };

    var post_req = http.request(post_options, res => {
        res.setEncoding('utf8');
        var returnData = "";
        res.on('data', chunk => {
            returnData += chunk;
        });
        res.on('end', () => {
            // this particular API returns a JSON structure:
            // returnData: {"usstate":"New Jersey","population":9000000}

            population = JSON.parse(returnData).population;

            callback(population);

        });
    });
    post_req.write(JSON.stringify(post_data));
    post_req.end();

}
