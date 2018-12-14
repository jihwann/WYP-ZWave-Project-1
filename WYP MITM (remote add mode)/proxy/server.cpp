#include "server.h"


server::server(QObject *parent) : QTcpServer(parent){}

void server::incomingConnection(qintptr socketDescriptor){
    QSslSocket *server_socket = new QSslSocket(this);
    server_socket->setSocketDescriptor(socketDescriptor);
    server_socket->setLocalCertificate(m_sslLocalCertificate);
    server_socket->setPrivateKey(m_sslPrivateKey);
    server_socket->setProtocol(m_sslProtocol);
    server_socket->startServerEncryption();
    this->addPendingConnection(server_socket);
}

const QSslCertificate &server::getSslLocalCertificate() const{
    return m_sslLocalCertificate;
}

const QSslKey &server::getSslPrivateKey() const{
    return m_sslPrivateKey;
}

QSsl::SslProtocol server::getSslProtocol() const{
    return m_sslProtocol;
}

void server::setSslLocalCertificate(const QSslCertificate &certificate){
    m_sslLocalCertificate = certificate;
}

bool server::setSslLocalCertificate(const QString &path, QSsl::EncodingFormat format){
    QFile certificateFile(path);

    if (!certificateFile.open(QIODevice::ReadOnly))
        return false;

    m_sslLocalCertificate = QSslCertificate(certificateFile.readAll(), format);
    return true;
}


void server::setSslPrivateKey(const QSslKey &key){
    m_sslPrivateKey = key;
}

bool server::setSslPrivateKey(const QString &fileName, QSsl::KeyAlgorithm algorithm, QSsl::EncodingFormat format, const QByteArray &passPhrase){
    QFile keyFile(fileName);

    if (!keyFile.open(QIODevice::ReadOnly))
        return false;

    m_sslPrivateKey = QSslKey(keyFile.readAll(), algorithm, format, QSsl::PrivateKey, passPhrase);
    return true;
}

void server::setSslProtocol(QSsl::SslProtocol protocol){
    m_sslProtocol = protocol;
}
