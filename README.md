WebsiteCustomer chat is an app that lets you and your customers to communicate via website chat.

It's Asp .Net MVC on net6.0 compatibile with windows/linux (tested) and mac (not tested yet);


Installation instruction - 

Requeriments:
- mysql database, other db engines support coming soon ( probably with swap to entity framework from raw sql queries)
- net framework 6.0
- SSL certificate - for standalone app hosting ( best way is to hide app behind <a href="#proxy" >proxy configuration</a>)


Installation

- unpack files from release in directory or compile app on your own
- Install MySql server and create database, user and assign All privileges on db to user
- start application with following command -
    linux:
  ./WebsiteCustomerChatMVC --urls='https://urdomain.com:<urPort> - can be 443 for standalone, or any for <a href="#proxy"> Proxied setup</a>

- Create Administrator account  
![image](https://github.com/JohnyWander/WebsiteCustomerChat/assets/98389805/85fcad6b-d1c1-42f6-b95c-0a1cc012bec3)

- Configure database
![image](https://github.com/JohnyWander/WebsiteCustomerChat/assets/98389805/e81d58a4-c530-47c7-9beb-d88d8a45285e)

<h2 id="proxy">Setting up proxy for application</h2>

1. lUse chat server with command line
--urlshttps://<server name/ip/localhost>:<port to listen on>

-Apache 

create virtualhost on your server domain/subdomain name or ip
<VirtualHost 10.10.10.10:443>
SSLProxyEngine On
ProxyPass / https://localhost:5000/
ProxyPassReverse / https://localhost:5000/
 #-- other ssl config




</VirtualHost>

