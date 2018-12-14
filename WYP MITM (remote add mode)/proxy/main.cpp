#include <QCoreApplication>
#include "proxy.h"
//final
int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);
    qSetMessagePattern("%{time h:mm:ss.zzz} %{threadid} [%{file}:%{line}] %{message}"); //using debuging
    proxy *pp = new proxy;
    pp->proxy_start();
    return a.exec();
}
