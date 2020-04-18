
    $(document).ready(function () {
             
        var data = JSON.stringify({
        Token: localStorage.getItem("BrowserToken")
})
            ajaxRequest("POST", "/Home/UpdateToken", data, "json").then(function (result) {
        console.log(result);
    });
});

