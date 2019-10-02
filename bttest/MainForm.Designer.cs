using MySql.Data.MySqlClient;

namespace bttest
{
    partial class MainForm
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelRecDir = new System.Windows.Forms.Label();
            this.chooseServer = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.labelPath = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.sendStatusInfo = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonListen = new System.Windows.Forms.Button();
            this.ListenerStatusInfo = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelRecDir
            // 
            this.labelRecDir.AutoSize = true;
            this.labelRecDir.Location = new System.Drawing.Point(192, 279);
            this.labelRecDir.Name = "labelRecDir";
            this.labelRecDir.Size = new System.Drawing.Size(106, 17);
            this.labelRecDir.TabIndex = 0;
            this.labelRecDir.Text = "请选择接收位置";
            this.labelRecDir.Visible = false;
            // 
            // chooseServer
            // 
            this.chooseServer.Location = new System.Drawing.Point(48, 69);
            this.chooseServer.Name = "chooseServer";
            this.chooseServer.Size = new System.Drawing.Size(138, 37);
            this.chooseServer.TabIndex = 1;
            this.chooseServer.Text = "选择蓝牙主机";
            this.chooseServer.UseVisualStyleBackColor = true;
            this.chooseServer.Click += new System.EventHandler(this.ChooseServer_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(48, 305);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 37);
            this.button2.TabIndex = 2;
            this.button2.Text = "选择发送文件：";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.selectFile_Click);
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(193, 319);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(92, 17);
            this.labelPath.TabIndex = 3;
            this.labelPath.Text = "发送文件信息";
            this.labelPath.Visible = false;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(48, 194);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(138, 31);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.Button3_Click);
            // 
            // sendStatusInfo
            // 
            this.sendStatusInfo.AutoSize = true;
            this.sendStatusInfo.Location = new System.Drawing.Point(193, 201);
            this.sendStatusInfo.Name = "sendStatusInfo";
            this.sendStatusInfo.Size = new System.Drawing.Size(92, 17);
            this.sendStatusInfo.TabIndex = 5;
            this.sendStatusInfo.Text = "发送状态信息";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(48, 275);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(138, 24);
            this.button3.TabIndex = 6;
            this.button3.Text = "选择接受位置：";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            // 
            // buttonListen
            // 
            this.buttonListen.Location = new System.Drawing.Point(48, 249);
            this.buttonListen.Name = "buttonListen";
            this.buttonListen.Size = new System.Drawing.Size(123, 23);
            this.buttonListen.TabIndex = 7;
            this.buttonListen.Text = "开始/停止监听";
            this.buttonListen.UseVisualStyleBackColor = true;
            this.buttonListen.Visible = false;
            // 
            // ListenerStatusInfo
            // 
            this.ListenerStatusInfo.AutoSize = true;
            this.ListenerStatusInfo.Location = new System.Drawing.Point(192, 249);
            this.ListenerStatusInfo.Name = "ListenerStatusInfo";
            this.ListenerStatusInfo.Size = new System.Drawing.Size(92, 17);
            this.ListenerStatusInfo.TabIndex = 8;
            this.ListenerStatusInfo.Text = "监听状态信息";
            this.ListenerStatusInfo.Visible = false;
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(192, 79);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(92, 17);
            this.labelAddress.TabIndex = 9;
            this.labelAddress.Text = "蓝牙主机信息";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelAddress);
            this.Controls.Add(this.ListenerStatusInfo);
            this.Controls.Add(this.buttonListen);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.sendStatusInfo);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chooseServer);
            this.Controls.Add(this.labelRecDir);
            this.Name = "MainForm";
            this.Text = "蓝牙数据同步";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRecDir;
        private System.Windows.Forms.Button chooseServer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label sendStatusInfo;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonListen;
        private System.Windows.Forms.Label ListenerStatusInfo;
        private System.Windows.Forms.Label labelAddress;
    }
}

