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