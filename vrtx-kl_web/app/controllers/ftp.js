const credentials = require('../data/credentials.json');
const PromiseFtp = require('promise-ftp');

var fs = require('fs');
var dirlist

module.exports = {
    listDir: async() => {
        await getDirList('/htdocs/loggers');
        return dirlist;
    },
    getSysFile: async(mac) => {
        await getDirList('htdocs/loggers/' + mac + '/syslog');
        return dirlist;
    },
    getLogList: async(mac) => {
        await getDirList('htdocs/loggers/' + mac);
        return dirlist;
    },
    getFile: async(path, filename) => {
        var ftp = new PromiseFtp();
        await ftp.connect({host: 'ftpupload.net', user: credentials.ftpuser, password: credentials.ftppass})
        .then(function () {
            return ftp.get(path + filename);
        }).then(function (stream) {
            return new Promise(function (resolve, reject) {
                stream.once('close', resolve);
                stream.once('error', reject);
                stream.pipe(fs.createWriteStream('temp/' + filename));
          });
        }).then(function () {
            ftp.end();
        });
    }
}

async function getDirList(path) {
    var ftp = new PromiseFtp();
    await ftp.connect({host: 'ftpupload.net', user: credentials.ftpuser, password: credentials.ftppass})
    .then(function () {
        return ftp.list(path);
    }).then(function (list) {
        dirlist = list;
        ftp.end();
    });
}