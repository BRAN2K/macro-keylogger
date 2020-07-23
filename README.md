
## Macro-Keylogger

### Sobre o projeto

ESTE PROJETO NÃO É MALICIOSO E TEM INTENÇÕES DIDÁTICAS. <br/>
O projeto consiste na implementação de um keylogger embutido em um software de macro com o intuito de camuflar as intenções do programa e roubar os dados dos usuários (vítimas) que utilizarem o software. As informações capturadas envolvem tudo que o usuário digita em suas aplicações e informações sobre o seu computador.

---

### Pré-requisitos e recursos utilizados

Foi utilizado C# para a implementação do Keylogger. Destaque para as seguintes bibliotecas:
- [FluentFTP](https://github.com/robinrodricks/FluentFTP), utilizada para tratar as requisições FTP.
- Windows DLL: <br/>
  - user32.dll, responsável pelos [Hooks](https://docs.microsoft.com/en-us/windows/win32/winmsg/hooks) <br/>
  - kernel32.dll, responsável pelos módulos dos Hooks <br/>

Foi utilizado nodejs para a implementação do back-end do website (painel de controle), além dos recursos de html/css. Destaque para as seguintes bibliotecas:

- Node.js:
  - [Express](https://github.com/expressjs/express);
  - [Consign](https://github.com/jarradseers/consign);
  - [Promise FTP](https://github.com/realtymaps/promise-ftp).

- HTML:
  - [Bootstrap](https://getbootstrap.com/docs/4.5/getting-started/introduction/)
  - [Font Awesome](https://fontawesome.com/)

---

### Passo a passo de implementação
1. Pesquisa dos códigos-fonte de keyloggers em C/C++ e C#;
2. Estudo de como os códigos-fonte funcionavam;
3. Implementação do algoritmo de keylogger em C#;
4. Implementação do painel de controle (website) estruturado pelo Node.js.

---

### Tabela de Conteúdos
- [Bugs/Problemas conhecidos](#bugs)
- [Imagens/Screenshots](#imagens)
- [Instalação](#instalação)
- [Execução](#execução)
- [Implementações Futuras](#implementações)
- [Autores](#autores)
- [Contribuidores](#contribuidores)

---

## Bugs
- Caso o servidor FTP fique indisponível a aplicação C# e a Node.js não se conectarão ao mesmo, não enviando logs e gerando exceções nas requisições FTP na aplicação Node.js;
- Windows Defender reconhece como vírus;

---

### Imagens
> Login do painel de controle <br/>
![Imagem](https://cdn.discordapp.com/attachments/330870742143205378/735963777303969859/unknown.png) <br/><br/>

> Painel de controle com acesso aos endereços MAC infectados <br/>
![Imagem](https://cdn.discordapp.com/attachments/330870742143205378/735963901065429102/unknown.png) <br/><br/>

> Arquivos disponíveis para download do usuário infectado <br/>
![Imagem](https://cdn.discordapp.com/attachments/330870742143205378/735964047366815784/unknown.png) <br/><br/>

---

### Instalação
1. Abra a pasta "keylogger";
2. Execute o "setup.exe";
3. Confirme as janelas seguintes.

---

### Execução
Quando instalado, o Keylogger irá abrir em sequência e juntamente a isso criará um registro no caminho `\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run`, definindo que o mesmo abrirá com a inicialização do Windows 

---

### Implementações
- Clipboard listener. Responsável por monitorar a movimentação do clipboard (crtl+c, ctrl+x);
- Screenlogger;
- Deixar menos detectável;
- Refatoração dos métodos de acesso FTP;
- Exibir e formatar no painel de controle os aruivos de log;
- Macro para mascarar o Keylogger

---

### Autores

* Guilherme Brante ([@brantenosh](https://github.com/cavebran))
* Lucca Marques ([@yllumi](https://github.com/luccamapt))

---

### Contribuidores

* Leonardo Nogueira ([@luker](https://github.com/leonogc))
* Guilherme de Paula ([@sarky](https://github.com/gpsx))
