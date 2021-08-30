const Utils = require('../view-manager/Utils')

const LoginController = {
    section:{
        selector: '.login-section-js'
    },
    init(){
        if (!Utils.checkSection(this.section.selector)){
            return false;
        }else{
            this.loginHadler();
        }
    },
    loginHadler(){
      
    }
}
module.exports = LoginController;