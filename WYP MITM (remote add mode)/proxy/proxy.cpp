#include "proxy.h"
#include <QCoreApplication>


//void end_action(QSslSocket *proxyserver_, QSslSocket *proxyclient_, QByteArray message){
//    if(proxyserver_->write(message)==-1)
//    {
//        qDebug() << "Write failed : ";
//        exit(0);
//    }
//    if(!proxyserver_->waitForBytesWritten()){
//        qDebug() << "WaitFor failed : ";
//        exit(0);
//    }
//}

//void disconnect(QSslSocket *proxyserver_, QSslSocket *proxyclient_){
//    proxyclient_->disconnectFromHost();
//    proxyserver_->disconnectFromHost();
//    qDebug() << "Disconnected";
//}

void dump(char *direction_type, QByteArray &message){
    QString s;
    printf(" %s %d",direction_type, message.size());
    for(int i=0; message.size()>i; i++){

        if(i%32==0){
            printf("%s\n",qPrintable(s));
            s="";
        }
        u_char c=(u_char)message.at(i);
        if(isprint(c))
            s+=c;
        else
            s+='.';
        printf("%02X ", c);
    }
    printf("%s\n",qPrintable(s));
    printf("\n");
}

class S_CThread : public QThread{
public:
    QSslSocket *proxyserver_;
    QSslSocket *proxyclient_;
    S_CThread(QSslSocket *proxy_server, QSslSocket *proxy_client){
        proxyserver_=proxy_server;
        proxyclient_=proxy_client;
    }

    void run() override{
        while(true){
            if(!proxyclient_->waitForReadyRead(-1))    // Wait until some data is received, 5000 ms timeout (-1 doesn't work here)
            {
                qDebug().nospace() << "ERROR: could not receive message (" << qPrintable(proxyclient_->errorString()) << ")";
                break;
            }
            QByteArray message = proxyclient_->readAll();    // Read message
            dump("Server To Client", message);

            if(proxyserver_->write(message)==-1)
            {
                qDebug() << "Write failed : ";
                break;
            }
            if(!proxyserver_->waitForBytesWritten()){
                qDebug() << "WaitFor failed : ";
                break;
            }
        }
        proxyclient_->disconnectFromHost();
        proxyserver_->disconnectFromHost();
        qDebug() << "Disconnected";
    }
};

class C_SThread : public QThread{
public:
    QSslSocket *proxyserver_;
    server *sslserver;
    QSslSocket *proxyclient_;
    C_SThread(QObject *parent,  server *ssl_server, QSslSocket *proxy_server) : QThread(parent){
        proxyserver_=proxy_server;
        sslserver=ssl_server;
        proxyclient_=new QSslSocket(this);
        proxyclient_->setPeerVerifyMode(QSslSocket::VerifyNone);
    }
    ~C_SThread()override{}
    void run()override{
        QString hostName = "192.168.43.157";
        quint16 port = 12345;
        proxyclient_->connectToHostEncrypted(hostName, port);

        if(!proxyclient_->waitForConnected()){
            qDebug() << "client waitForConnected";
        }

        if(!proxyclient_->waitForEncrypted()){
            qDebug() << "client waitForEncrypted";
        }

        S_CThread *scThread = new S_CThread(proxyserver_,proxyclient_);
        scThread->start();

        while(true)
        {
            if(!proxyserver_->waitForReadyRead(-1))    // Wait until some data is received, 5000 ms timeout (-1 doesn't work here)
            {
                qDebug().nospace() << "ERROR: could not receive message (" << qPrintable(proxyserver_->errorString()) << ")";
                break;
            }
            QByteArray message = proxyserver_->readAll();    // Read message

            dump("Client To Server",message);

            if(proxyclient_->write(message)==-1)
            {
                qDebug() << "Write failed : ";
                break;
            }
            if(!proxyclient_->waitForBytesWritten()){
                qDebug() << "WaitFor failed : ";
                break;
            }
        }
        proxyclient_->disconnectFromHost();
        proxyserver_->disconnectFromHost();
        qDebug() << "Disconnected";
    }
};

proxy::proxy(QObject *parent) : QObject(parent)
{

}

void proxy::proxy_start(){
    QHostAddress address = QHostAddress::Any;
    quint16 port = 12345;
    server set_server;
    set_server.setSslLocalCertificate("/root/Desktop/certificate/private.crt");
    set_server.setSslPrivateKey("/root/Desktop/certificate/private.key");
    set_server.setSslProtocol(QSsl::TlsV1_2);

    if(set_server.listen(address, port)){
        qDebug().nospace() << "Now listening on " << qPrintable(address.toString()) << ":" << port;
        qDebug() << "New connection";
    }
    else
        qDebug().nospace() << "ERROR: could not bind to " << qPrintable(address.toString()) << ":" << port;

    if (set_server.waitForNewConnection(-1)){
        qDebug() << "waitForNewConnection";
        QSslSocket *proxyServer = dynamic_cast<QSslSocket*>(set_server.nextPendingConnection());
        C_SThread *csThread = new C_SThread(nullptr, &set_server, proxyServer);
        csThread->start();
        QThread::sleep(10000);
    }
    this->deleteLater();
    QThread::currentThread()->quit();
    qApp->exit();
}

