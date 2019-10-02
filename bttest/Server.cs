using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bttest
{
    class Server
    {
       private BluetoothRadio radio = null;//蓝牙适配器  
       
        ObexListener listener = null;//监听器  
        string recDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//接受文件存放目录  
        Thread listenThread; //接收线程  
        public void getRadioInfo()
        {
            radio = BluetoothRadio.PrimaryRadio;//获取当前PC的蓝牙适配器  
            //CheckForIllegalCrossThreadCalls = false;//不检查跨线程调用  
            if (radio == null)//检查该电脑蓝牙是否可用  
            {
                Console.WriteLine("这个电脑蓝牙不可用！提示");
            }
        }

        public void OperateListener()
        {
            if (listener == null || !listener.IsListening)
            {
                radio.Mode = RadioMode.Discoverable;//设置本地蓝牙可被检测  
                listener = new ObexListener(ObexTransport.Bluetooth);//创建监听  
                listener.Start();
                if (listener.IsListening)
                {
                    Console.WriteLine("开始监听");
                    listenThread = new Thread(receiveFile);//开启监听线程  
                    listenThread.Start();
                }
            }
            else
            {
                listener.Stop();
                Console.WriteLine("停止监听");
            }


        }
        private void receiveFile()//收文件方法  
        {
            ObexListenerContext context = null;
            ObexListenerRequest request = null;
            while (listener.IsListening)
            {
                context = listener.GetContext();//获取监听上下文  
                if (context == null)
                {
                    break;
                }
                request = context.Request;//获取请求  
                string uriString = Uri.UnescapeDataString(request.RawUrl);//将uri转换成字符串  
                string recFileName = recDir + uriString;
                request.WriteFile(recFileName);//接收文件  
                Console.WriteLine("收到文件" + uriString.TrimStart(new char[] { '/' }));
            }
        }

    }
}
