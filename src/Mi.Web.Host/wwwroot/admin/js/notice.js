"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/noticeHub").build();

connection.on("ReceiveMessage", function (title, content) {
    window.toast.info({ title: title, message: content, position: 'topCenter' });
});

connection.start().then(() => {
    let intervalTime = 1000 * 60 * 1;//minute
    let miUser = JSON.parse(localStorage.getItem('mi-user'))
    if (miUser && miUser.userId) {
        setInterval(function () {
            connection.invoke("SendMessage", miUser.userId.toString(), "", "").catch(function (err) {
                return console.error(err.toString());
            });
        }, intervalTime)
    }
}).catch(function (err) {
    return console.error(err.toString());
});