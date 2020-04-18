$(document).ready(function () {
    $('.logoutSession').click(function () {
        alert();
        ajaxRequest("Get", "/User/Logout", "", "json").then(function (result) {
            if (result != "Failed") {                
                deleteToken();
                window.location.href = "/Home";
            }
        });
    });
});


function deleteToken() {
    // Delete Instance ID token.
    // [START delete_token]   
    messaging.getToken().then((currentToken) => {
        messaging.deleteToken(currentToken).then(() => {            
            console.log('Token deleted.');
            setTokenSentToServer(false);
            // [START_EXCLUDE]
            // Once token is deleted update UI.
           // resetUI();
            window.location.href = "/Home";
            // [END_EXCLUDE]
        }).catch((err) => {
            console.log('Unable to delete token. ', err);
        });
        // [END delete_token]
    }).catch((err) => {
        console.log('Error retrieving Instance ID token. ', err);
        showToken('Error retrieving Instance ID token. ', err);
    });
}

function resetUI() {
    clearMessages();
    showToken('loading...');
    // [START get_token]
    // Get Instance ID token. Initially this makes a network call, once retrieved
    // subsequent calls to getToken will return from cache.
    messaging.getToken().then((currentToken) => {
        if (currentToken) {
            sendTokenToServer(currentToken);
            updateUIForPushEnabled(currentToken);
        } else {
            // Show permission request.
            console.log('No Instance ID token available. Request permission to generate one.');
            // Show permission UI.
            updateUIForPushPermissionRequired();
            setTokenSentToServer(false);
        }
    }).catch((err) => {
        console.log('An error occurred while retrieving token. ', err);
        showToken('Error retrieving Instance ID token. ', err);
        setTokenSentToServer(false);
    });
    // [END get_token]
}
