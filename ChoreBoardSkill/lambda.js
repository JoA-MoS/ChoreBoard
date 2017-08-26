// alexa-cookbook sample code

// There are three sections, Text Strings, Skill Code, and Helper Function(s).
// You can copy and paste the entire file contents as the code for a new Lambda function,
//  or copy & paste section #3, the helper function, to the bottom of your existing Lambda code.


// 1. Text strings =====================================================================================================
//    Modify these strings and messages to change the behavior of your Lambda function

var myRequest;
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
            this.emit(':tell', 'Your chors today are ' + sayArray(myResult, 'and'));
        }
        );

    },

    'CreateChore': function () {
        var chore = this.event.request.intent.slots.Chore.value;
        myRequest = chore;
        httpPost(myRequest, myResult => {
            console.log("sent     : " + JSON.stringify(myRequest));
            console.log("received : " + JSON.stringify(myResult));

            this.emit(':tell', 'The ' + myResult + ' chore was created');

        }
        );

    }
};


//    END of Intent Handlers {} ========================================================================================
// Helper Functions  =================================================================================================


var http = require('http');
var qs = require('querystring');

function httpGet(myData, callback) {

    var options = {
        host: '34.209.13.220',
        port: 80,
        path: '/api/boards/1/chores',
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
            console.log(JSON.stringify(returnData))
            var choreObj = JSON.parse(returnData)
            var choreArr = choreObj.map(function (a) { return a.name; });
            callback(choreArr);  // this will execute whatever function the caller defined, with one argument
        });

    });
    req.end();

}


function httpPost(myData, callback) {

    // GET is a web service request that is fully defined by a URL string
    // Try GET in your browser:
    // http://cp6gckjt97.execute-api.us-east-1.amazonaws.com/prod/stateresource?usstate=New%20Jersey


    var post_data = {
        name: myData,
        boardId: 1
    }
    console.log(JSON.stringify(post_data));
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

            callback(JSON.parse(returnData).name);

        });
    });
    post_req.write(JSON.stringify(post_data));
    post_req.end();

}

// ========================================================
//  helper functions
// ========================================================

function sayArray(myData, andor) {
    // the first argument is an array [] of items
    // the second argument is the list penultimate word; and/or/nor etc.
    var listString = [myData.slice(0, -1).join(', '), myData.slice(-1)[0]].join(myData.length < 2 ? '' : ' ' + andor + ' ');
    return (listString);
}
