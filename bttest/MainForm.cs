using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bttest
{
    public partial class MainForm : Form
    {
        BluetoothRadio radio = null;//蓝牙适配器  
        string sendFileName = null;//发送文件名  
        BluetoothAddress sendAddress = null;//发送目的地址  
        DBFileService dbFileService = null;
        Thread  sendThread;//发送/接收线程  
        private bool isBlueToothAble = false;

        private string rootPath = @"c:\bt\";

        public bool IsBlueToothAble
        {
            get
            {
                return isBlueToothAble;
            }
        }
        public MainForm()
        {
            InitializeComponent();
            //PrintDeviceInfo();
            radio = BluetoothRadio.PrimaryRadio;//获取当前PC的蓝牙适配器  
            if (radio == null) {
                isBlueToothAble = false;
                MessageBox.Show("这个电脑蓝牙不可用,请先打开蓝牙，再打开应用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                isBlueToothAble = true;
                radio.Mode = RadioMode.Connectable;
                CheckForIllegalCrossThreadCalls = false;//不检查跨线程调用  
                dbFileService = new DBFileService();
            }
            this.buttonSend.Enabled = false;
            //this.insertSyncDBTime();
           // this.getSyncDBTime();
        }


        private void bakUpFileByTabName(string fileName)
        {
            try
            {
                string dateTimeStr = DateTime.Now.ToString("yyyyMMddhhmm");
                string newDirectory = @"c:\bt" + dateTimeStr;
                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                }
                Console.WriteLine(dateTimeStr);
                //string filename = "tbuser.json";
                File.Move(rootPath + fileName, newDirectory + @"\" + fileName + ".bak");
            }
            catch (System.Exception ex)
            {
                ErrorLog.WriteLog(ex);
            }
        }
        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sendFileName = dialog.FileName;//设置文件名  
                labelPath.Text = Path.GetFileName(sendFileName);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            sendThread = new Thread(sendFile);//开启发送文件线程  
            sendThread.Start();
        }

        private void sendFile()//发送文件方法  
        {
            if (sendAddress == null)
            {
                MessageBox.Show("请先选择发送主机，再选择发送文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    if (Directory.Exists(rootPath))
                    {
                        Directory.Delete(rootPath, true);
                    }
                    Directory.CreateDirectory(rootPath);
                    dbFileService.exportDBdata2Txt(rootPath);
                
                sendStatusInfo.Text = "开始发送!";
                 foreach (string tbName in DBFileService.tableNames) {
                //string tbName = "C:\\bt\\tbbom.txt";
                    //string tbName = "tbuser";
                    //if (tbName.Equals("tbuser"))
                    //{
                        string sendFileName = String.Format("C:\\bt\\{0}.txt", tbName);
                        if (File.Exists(sendFileName))
                        {
                            sendFile2Server(sendFileName, tbName);
                        }
                        else
                        {
                            ErrorLog.WriteErrorMessage("导出文件不存在：" + sendFileName);
                        }
                     //}
                 }
                sendStatusInfo.Text = "发送完成!";
                    insertSyncDBTime();
                buttonSend.Enabled = true;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("连接异常：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLog.WriteLog(ex);
                    throw new Exception();
                }
            }
        }

        private void insertSyncDBTime()
        {
            dbFileService.insertSyncDBTime();
        }

        private void getSyncDBTime() {
            //string syncTime = dbFileService.getSyncDBTime();
            //Console.WriteLine(syncTime + "=================================");
        }

        private void sendFile2Server(string fileName, string tbName) {
            ObexWebRequest request = new ObexWebRequest(sendAddress, Path.GetFileName(fileName));//创建网络请求  
            WebResponse response = null;
            try
            {
                buttonSend.Enabled = false;
                request.ReadFile(fileName);//发送文件  
                
                response = request.GetResponse();//获取回应  
               
            }
            catch (System.Exception ex)
            {
                ErrorLog.WriteLog(ex);
                MessageBox.Show("发送失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sendStatusInfo.Text = "发送失败!";
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                this.bakUpFileByTabName(tbName+".txt");
            }
        }

        void PrintDeviceInfo()
        {
            BluetoothRadio bluetoothRadio = BluetoothRadio.PrimaryRadio;
            if (bluetoothRadio == null)
            {
                Console.WriteLine("没有找到本机蓝牙设备!");
            }
            else
            {
                Console.WriteLine("ClassOfDevice: " + bluetoothRadio.ClassOfDevice);
                Console.WriteLine("HardwareStatus: " + bluetoothRadio.HardwareStatus);
                Console.WriteLine("HciRevision: " + bluetoothRadio.HciRevision);
                Console.WriteLine("HciVersion: " + bluetoothRadio.HciVersion);
                Console.WriteLine("LmpSubversion: " + bluetoothRadio.LmpSubversion);
                Console.WriteLine("LmpVersion: " + bluetoothRadio.LmpVersion);
                Console.WriteLine("LocalAddress: " + bluetoothRadio.LocalAddress);
                Console.WriteLine("Manufacturer: " + bluetoothRadio.Manufacturer);
                Console.WriteLine("Mode: " + bluetoothRadio.Mode);
                Console.WriteLine("Name: " + bluetoothRadio.Name);
                Console.WriteLine("Remote:" + bluetoothRadio.Remote);
                Console.WriteLine("SoftwareManufacturer: " + bluetoothRadio.SoftwareManufacturer);
                Console.WriteLine("StackFactory: " + bluetoothRadio.StackFactory);
            }
            Console.ReadKey();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sendThread != null)
           {
               sendThread.Abort();
           }
        }


        private void ChooseServer_Click(object sender, EventArgs e)
        {
            SelectBluetoothDeviceDialog dialog = new SelectBluetoothDeviceDialog();
            dialog.ShowRemembered = true;//显示已经记住的蓝牙设备  
            dialog.ShowAuthenticated = true;//显示认证过的蓝牙设备  
            dialog.ShowUnknown = true;//显示位置蓝牙设备  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sendAddress = dialog.SelectedDevice.DeviceAddress;//获取选择的远程蓝牙地址  
                labelAddress.Text = "地址:" + sendAddress.ToString() + "    设备名:" + dialog.SelectedDevice.DeviceName;
                //BluetoothEnd.
                BluetoothClient Blueclient = new BluetoothClient();
                BluetoothDeviceInfo[] Devices = Blueclient.DiscoverDevices();
                //Blueclient.SetPin(sendAddress, "288063");
                Blueclient.Authenticate = false;
                Blueclient.Connect(sendAddress, BluetoothService.Handsfree);
                this.buttonSend.Enabled = true;
                // Blueclient.EndConnect();
            }
        }

        private void selectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sendFileName = dialog.FileName;//设置文件名  
                labelPath.Text = Path.GetFileName(sendFileName);
            }
        }


    }
}
