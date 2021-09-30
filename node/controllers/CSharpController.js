const CSharpController = {
    init(io, clientC) {
        this.reciveDataCSharp(io, clientC)
    },
    reciveDataCSharp(io, clientC){
        clientC.on("data", function (data) {
            dataDecrypt = data.toString("utf8");
            const idType = CSharpController.getIdType(dataDecrypt);
            const parameters = CSharpController.getParameters(dataDecrypt);
        
            CSharpController.sendDataToSocketIO(io, idType, parameters);
        });
    },
    sendDataToSocketIO(io, idType, parameters) {
        const client_uid = parameters[0];

        switch (idType) {
            case "entrarSala":
                io.emit("entrarSala-" + client_uid);
                break;
            case "salirSala":
                io.emit("salirSala-" + client_uid);
                break;
            case "entrarFlowerPower":
                io.emit("entrarFlowerPower-" + client_uid);
                break;
        }
    },
    getIdType(data) {
        let dataC = data.split("|");
        return dataC[0];
    },
    getParameters(data) {
        let dataC = data.split("|");
        let parameters = dataC[1].split(",");
        return parameters;
    }
}

module.exports = CSharpController;