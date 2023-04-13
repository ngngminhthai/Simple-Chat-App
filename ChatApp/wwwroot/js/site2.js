var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();


connection.on("ReceiveMessage", function (content, userId, userName) {
    var text = '';
    var ul = $('.chat-history-message');

    if (userId == currentUserId) {
        text += `
                     <li class="clearfix">
                                        <div class="message-data text-right">
                                            <span class="message-data-time">${userName}</span>
                                        </div>
                                        <div class="message other-message float-right">
                                            ${content}
                                        </div>
                      </li>
        `
    }
    else text += `
        <li class="clearfix">
                                        <div class="message-data">
                                            <span class="message-data-time">${userName}</span>
                                        </div>
                                        <div class="message my-message">${content}</div>
                                    </li>
    `
    ul.append(text);
})

connection.on("Connected", function (userName) {
    var userNamesArray = userName.split("|");
    userNamesArray.forEach(function (name) { // loop through each user name using forEach() loop
        var statusDiv = document.getElementById(name); // get the div element for the current user name
        if (statusDiv) statusDiv.innerHTML = '<i class="fa fa-circle online"></i> Online'; // set the inner HTML for the div element
    });
});

connection.on("Disconnected", function (userName) {
    var userNamesArray = userName.split("|");
    userNamesArray.forEach(function (name) { // loop through each user name using forEach() loop
        var statusDiv = document.getElementById(name); // get the div element for the current user name
        if (statusDiv) statusDiv.innerHTML = '<i class="fa fa-circle offline"></i> Offline'; // set the inner HTML for the div element
    });
});


$('.input-group-text.sendbtn').click(function () {

    var msg = $('.input-message').val()

    connection.invoke("SendMsg", groupName, msg, isGroupChat, groupId).then(function (result) {

    });
});


connection.start().then(function () {
    console.log("SignalR connected successfully!");
    connection.invoke("Join", groupName).then(function (result) {

    });
}).catch(function (error) {
    console.error("Error connecting to SignalR: " + error);
});

