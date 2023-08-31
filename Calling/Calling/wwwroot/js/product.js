"use strict";
dubgger;
var connection = new signalR.HubConnectionBuilder().withUrl("/productHub").build();

document.getElementById("sendButton").disabled = true;


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var productName = document.getElementById("productName").value;
    connection.invoke("SendProduct", productName).then(function () {
        document.getElementById("productName").value = "";
        document.getElementById("productName").focus();
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("resetButton").addEventListener("click", function (event) {
    connection.invoke("ResetProduct").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
