"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, group) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `Group: ${group} - ${user} says ${message}`;
});

connection.on("Send", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = message;
});

connection.on("ReceiveLog", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = message;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", async function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var group = document.getElementById("groupSelect").value;
    var customGroup = document.getElementById("customGroupInput").value;
    group = group == "Custom" ? customGroup : group;

    var previousGroup = await connection.invoke("GetGroup").catch(function (err) {
        console.error(err.toString());
    });
    
    if (previousGroup !== group && previousGroup != null) {
        connection.invoke("RemoveFromGroup", previousGroup, user).catch(function (err) {
            console.error(err.toString());
        });

        // clear the chat
        document.getElementById("messagesList").innerHTML = "";

        connection.invoke("AddToGroup", group, user).catch(function (err) {
            console.error(err.toString());
        });
    } else if (previousGroup == null) {        
        connection.invoke("AddToGroup", group, user).catch(function (err) {
            console.error(err.toString());
        });
    }

    connection.invoke("SendMessage", user, message, group).catch(function (err) {
        console.error(err.toString());
    });
    event.preventDefault();
});