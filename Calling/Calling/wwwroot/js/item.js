"use strict";
debugger;

var connection = new signalR.HubConnectionBuilder().withUrl("/itemHub").build();

document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var productName = document.getElementById("productName").value;
    var productDescription = $("#productDescription").val();
    var productCost = $("#productCost").val();
    var stock = $("#stock").val();
    var listItem = document.createElement("li");
    listItem.innerHTML = `
        <strong>Product Name:</strong> ${productName}<br>
        <strong>Description:</strong> ${productDescription}<br>
        <strong>Cost:</strong> ${productCost}<br>
        <strong>Stock:</strong> ${stock}
    `;
    connection.invoke("SendProduct", listItem.outerHTML).then(function () {
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
