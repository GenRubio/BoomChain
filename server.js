const { Console } = require("console");
const express = require("express");
const app = express();
const server = require("http").createServer(app);
var net = require("net");
var clientC = new net.Socket();

const io = require("socket.io")(server, {
    cors: { origin: "*" },
});

///Client HTML
io.on("connection", (socket) => {
    socket.on("change-ninja", (data) => {
        clientC.write("change-ninja|" + data.token_uid + "," + data.cintaNinja);
    });

    socket.on("change-ficha-user", (data) => {
        clientC.write("change-ficha-user|" + data.token_uid + "," + data.ficha);
    });
});
/****************************************************************************** */
///Client C#
clientC.on("data", function (data) {
    dataDecrypt = data.toString("utf8");
    let idType = getIdType(dataDecrypt);
    let parameters = getParameters(dataDecrypt);

    sendDataHTML(idType, parameters);
});

function sendDataHTML(idType, parameters) {
    switch (idType) {
        case "entrarSala":
            io.emit("entrarSala-" + parameters[0], parameters[1]);
            break;
        case "salirSala":
            io.emit("salirSala-" + parameters[0], parameters[1]);
            break;
        case "entrarFlowerPower":
            io.emit("entrarFlowerPower-" + parameters[0]);
            break;
    }
}
/**************************************************************************** */
function getIdType(data) {
    let dataC = data.split("|");
    return dataC[0];
}

function getParameters(data) {
    let dataC = data.split("|");
    let parameters = dataC[1].split(",");
    return parameters;
}

clientC.on("close", function () {
    console.log("Connection whitch C# close");
});

server.listen(3000, () => {
    console.log("Socket server is running.");
});

clientC.connect(3300, "127.0.0.1", function () {});
