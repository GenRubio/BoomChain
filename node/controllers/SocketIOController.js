const SocketIOController = {
    init(io, clientC) {
        this.reciveDataSocketIO(io, clientC);
    },
    reciveDataSocketIO(io, clientC) {
        io.on("connection", (socket) => {
            socket.on("change-ninja", (data) => {
                const response = {
                    package: "change-ninja",
                    client_uid: data.token_uid,
                    parameters: [data.cintaNinja],
                };
                SocketIOController.prepairData(clientC, response);
            });

            socket.on("change-ficha-user", (data) => {
                const response = {
                    package: "change-ficha-user",
                    client_uid: data.token_uid,
                    parameters: [data.ficha],
                };
                SocketIOController.prepairData(clientC, response);
            });
        });
    },
    prepairData(clientC, response) {
        let data = response.package + "|" + response.client_uid;

        if (response.parameters.length > 0) {
            response.parameters.forEach(function (parameter) {
                data += "," + parameter;
            });
        }

        this.sendDataCSharp(clientC, data);
    },
    sendDataCSharp(clientC, data) {
        clientC.write(data);
    },
};

module.exports = SocketIOController;
