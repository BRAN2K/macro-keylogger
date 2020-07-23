const auth = require('../controllers/auth');
const ftp = require('../controllers/ftp');

var fs = require('fs');

module.exports = function(application){
	application.get('/index', async function(req, res) {
		if(auth.isLogged(req)){
			var dirlist = await ftp.listDir();
			res.render('../views/index', { dirlist });
		}
		else 
			res.redirect('/');
	});

	application.get('/user/:MAC_ADDRESS', async function(req, res) {
		if(auth.isLogged(req)) {
			var mac = req.params.MAC_ADDRESS;
			var sys = await ftp.getSysFile(mac);
			var loglist = await ftp.getLogList(mac);
			var log = [];

			for(l of loglist) {
				if(l.name != '..' && l.name != '.' && l.name != 'syslog'){
					log.push({
						'id': l.name.split('#')[0],
						'name': l.name,
						'type': l.type
					})
				}
			}
			log = log.sort(compareId);

			res.render('../views/userinfo', {mac, sys, log});
		}	
		else 
			res.redirect('/');
	});

	application.post('/download', async function(req, res) {
		if(auth.isLogged(req)) {
			if(req.body.filename == 'sys_data_capture.log')
				await ftp.getFile('htdocs/loggers/' + req.body.mac_address + '/syslog/', req.body.filename);
			else if(!fs.existsSync('temp/' + req.body.filename))
				await ftp.getFile('htdocs/loggers/' + req.body.mac_address + '/', req.body.filename);
			
			res.download('temp/' + req.body.filename);
		}
		else 
			res.redirect('/');
	});

	application.get('/', function(req, res) {
		res.render('../views/login');
	});
	
	application.post('/login', function(req, res) {
		auth.login(req) ? res.redirect('/index') : res.redirect('/');
	})
}

function compareId(a, b) {
	let comparison = 0;
	a.id = parseInt(a.id); b.id = parseInt(b.id);

	if (a.id > b.id)
		comparison = 1;
	else if (a.id < b.id)
		comparison = -1;
	
	return comparison;
  }