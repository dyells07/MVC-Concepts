"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();

$(function () {
	connection.start().then(function () {
		alert('Connected to dashboardHub');

		

	}).catch(function (err) {
		return console.error(err.toString());
	});
});

