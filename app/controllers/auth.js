const credentials = require('../data/credentials.json');

module.exports = {
    login:(req) => {
        if(req.body.user == credentials.user && req.body.pass == credentials.password){
            req.session.user = req.body.user;
            return true;
        }
        else {
            return false;
        }
    },
    isLogged:(req) => {
        return req.session && req.session.user ? true : false;
    }
}