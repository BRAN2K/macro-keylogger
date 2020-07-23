const auth = require('../controllers/auth');
const ftp = require('../controllers/ftp');

var fs = require('fs');

module.exports = function(application){
	application.get('/index', async function(req, res) {
		if(auth.isLogged(req)){
			var dir = await ftp.listDir();
			var dirlist = []
			for(d of dir) {
				if(d.name != '..' && d.name != '.') {
					dirlist.push({
						'userdir': d.name,
						'mac': d.name.split('_')[0],
						'computer': d.name.split('_')[1],
						'type': d.type
					})
				}
			}
			res.render('../views/index', { dirlist });
		}
		else 
			res.redirect('/');
	});

	application.get('/user/:USER_DIR', async function(req, res) {
		if(auth.isLogged(req)) {
			var userdir = req.params.USER_DIR;
			var sys = await ftp.getSysFile(userdir);
			var loglist = await ftp.getLogList(userdir);
			var log = [];

			for(l of loglist) {
				if(l.name != '..' && l.name != '.' && l.type == '-'){
					log.push({
						'id': l.name.split('#')[0],
						'name': l.name,
						'type': l.type
					})
				}
			}
			log = log.sort(compareId);
			
			var information = {
				'userdir': userdir,
				'mac': userdir.split('_')[0]
			}
			res.render('../views/userinfo', {information, sys, log});
		}	
		else 
			res.redirect('/');
	});

	application.post('/download', async function(req, res) {
		if(auth.isLogged(req)) {
			if(req.body.filename == 'sys_data_capture.log')
				await ftp.getFile('htdocs/loggers/' + req.body.userdir + '/syslog/', req.body.filename);
			else if(!fs.existsSync('temp/' + req.body.filename))
				await ftp.getFile('htdocs/loggers/' + req.body.userdir + '/', req.body.filename);
			
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