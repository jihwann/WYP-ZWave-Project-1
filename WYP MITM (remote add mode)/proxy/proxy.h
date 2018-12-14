#ifndef PROXY_H
#define PROXY_H

#include <QObject>
#include <QHostAddress>
#include <QThread>
#include "server.h"

class proxy : public QObject
{
    Q_OBJECT
public:
    explicit proxy(QObject *parent = nullptr);
    void proxy_start();
};

#endif // PROXY_H
