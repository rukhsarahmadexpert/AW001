importScripts("https://www.gstatic.com/firebasejs/7.8.2/firebase-app.js")
importScripts("https://www.gstatic.com/firebasejs/7.8.2/firebase-messaging.js")

var firebaseConfig = {
    apiKey: "AIzaSyCPsGFzC4fUJw1_WYqH4wvuI3OZepPqiJw",
    authDomain: "awfuels.firebaseapp.com",
    databaseURL: "https://awfuels.firebaseio.com",
    projectId: "awfuels",
    storageBucket: "awfuels.appspot.com",
    messagingSenderId: "1089383880964",
    appId: "1:1089383880964:web:bbc53006ba3b1c27cdd851",
    measurementId: "G-YJQQC6ES6L"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function (payload) {

    var title = payload.data.Titles;
    var options = {
        body: payload.data.Messages,
        icon: 'logo-1.png',
        image: payload.data.image,
       // Code = payload.data.Code,
    };
    return self.registration.showNotification(title, options);
});

self.addEventListener('notificationclick', function (event) {
   // var code = event.notification.data.Code;

    event.notification.close();
    event.waitUntil(
        clients.openWindow("https://www.awfuel.com")
    );
});