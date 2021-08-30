const LoginController = require("./controllers/LoginController");
const ViewHandler = {
    init() {
        document.addEventListener("DOMContentLoaded", () => {
            this.onDocumentReady();
        });
    },

    onDocumentReady() {
        LoginController.init();
    },
};
module.exports = ViewHandler;
