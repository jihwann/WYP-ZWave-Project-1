#ifndef SERVER_H
#define SERVER_H
#include <QObject>
#include <QTcpServer>
#include <QSslSocket>
#include <QSslKey>
#include <QFile>
#include <QThread>

class server : public QTcpServer
{
    Q_OBJECT
public:
    explicit server(QObject *parent = nullptr);
    void run();
    const QSslCertificate &getSslLocalCertificate() const;
    const QSslKey &getSslPrivateKey() const;
    QSsl::SslProtocol getSslProtocol() const;
    void setSslLocalCertificate(const QSslCertificate &certificate);
    bool setSslLocalCertificate(const QString &path, QSsl::EncodingFormat format = QSsl::Pem);
    void setSslPrivateKey(const QSslKey &key);
    bool setSslPrivateKey(const QString &fileName, QSsl::KeyAlgorithm algorithm = QSsl::Rsa, QSsl::EncodingFormat format = QSsl::Pem, const QByteArray &passPhrase = QByteArray());
    void setSslProtocol(QSsl::SslProtocol protocol);

protected:
    void incomingConnection(qintptr socketDescriptor); //important !!

private:
    QSslCertificate m_sslLocalCertificate;
    QSslKey m_sslPrivateKey;
    QSsl::SslProtocol m_sslProtocol;
};

#endif // SERVER_H
