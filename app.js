/* importar as configurações do servidor */
var app = require('./config/server');

/* parametrizar a porta de escuta */
const port = (process.env.PORT || 80);
app.listen(port, function(){
	console.log('Servidor online: ' + port);
})