

function generateID(str) {
    var rd = '';
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (var i = 0; i < 2; i++) {
        rd += possible.charAt(Math.floor(Math.random() * possible.length));
    }     
    return  str + Date.now() + rd;
}


