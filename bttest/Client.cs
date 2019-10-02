using InTheHand.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bttest
{
    class Client
    {
        string sendFileName = null;//发送文件名  
        BluetoothAddress sendAddress = null;//发送目的地址  
        Thread sendThread;
    }
}
