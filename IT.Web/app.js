var firebaseConfig = {
    apiKey: "AIzaSyDI7yrYfmusUEwpVqjHoUkno9Xao6_VFPg",
    authDomain: "awfuel-trading.firebaseapp.com",
    databaseURL: "https://awfuel-trading.firebaseio.com",
    projectId: "awfuel-trading",
    storageBucket: "awfuel-trading.appspot.com",
    messagingSenderId: "284050157502",
    appId: "1:284050157502:web:9e6de3bd6e6862937313ef",
    measurementId: "G-WH79G3TD9L"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
//firebase.analytics();
const messaging = firebase.messaging();
messaging.requestPermission().then(function () {
    //console.log('have permission');
    if (isTokenSentToServer()) {
        console.log('Token already sent')
        messaging.getToken().then((currentTokens) =>
        {
            localStorage.setItem("BrowserToken", currentTokens);
            console.log(currentTokens);
        });
    }
    else {
        getRegisterToken();
    }    
    return messaging.getToken();
})
    //.then(function (token) {  
    //    console.log(token);
    //})
    .catch(function (err) {
        console.log('Error Occured' + err);
    })


function getRegisterToken() {
    messaging.getToken().then((currentToken) => {
        if (currentToken) {
            console.log('have permission');
            console.log(currentToken);           
            sendTokenToServer(currentToken);
            localStorage.setItem("BrowserToken", currentTokens);
            // updateUIForPushEnabled(currentToken);
        } else {
            // Show permission request.
            console.log('No Instance ID token available. Request permission to generate one.');
            // Show permission UI.
            updateUIForPushPermissionRequired();
            setTokenSentToServer(false);
        }
    }).catch((err) => {
        console.log('An error occurred while retrieving token. ', err);
        // showToken('Error retrieving Instance ID token. ', err);
        setTokenSentToServer(false);
    });
}

messaging.onMessage((payload) => {
   //   console.log('Message received. ', payload);
    // ...
    var title = payload.data.Titles;
    var options = {
        body: payload.data.Messages,
        icon: '~/logo-1.png',
        image: payload.data.image,
    };

    var myNotification = new Notification(title, options);

});


function isTokenSentToServer() {
    return window.localStorage.getItem('sentToServer') === '1';
}

function sendTokenToServer(currentToken) {
    if (!isTokenSentToServer()) {
        console.log('Sending token to server...');
        // TODO(developer): Send the current token to your server.
        setTokenSentToServer(true);
    } else {
        console.log('Token already sent to server so won\'t send it again ' +
            'unless it changes');
    }

}


function setTokenSentToServer(sent) {
    window.localStorage.setItem('sentToServer', sent ? '1' : '0');
}