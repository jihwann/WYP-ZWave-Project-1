namespace WYP_ZWave_Spoofing_Tool
{
    partial class MdiMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 
        
        private void InitializeComponent()
        {
            this.btn1 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.label_PortStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn7 = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn1
            // 
            this.btn1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn1.Location = new System.Drawing.Point(33, 33);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(171, 26);
            this.btn1.TabIndex = 0;
            this.btn1.Text = "1. Setup serial port";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.Btn1_Click);
            // 
            // btn2
            // 
            this.btn2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn2.Location = new System.Drawing.Point(161, 81);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(78, 32);
            this.btn2.TabIndex = 1;
            this.btn2.Text = "start";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn3
            // 
            this.btn3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn3.Location = new System.Drawing.Point(245, 81);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(78, 32);
            this.btn3.TabIndex = 2;
            this.btn3.Text = "stop";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn4
            // 
            this.btn4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn4.Location = new System.Drawing.Point(33, 130);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(272, 27);
            this.btn4.TabIndex = 3;
            this.btn4.Text = "3. Run command (Non security)";
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn5
            // 
            this.btn5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn5.Location = new System.Drawing.Point(33, 240);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(277, 61);
            this.btn5.TabIndex = 4;
            this.btn5.Text = "4. Run command (S0, S2)";
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // btn6
            // 
            this.btn6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn6.Location = new System.Drawing.Point(321, 309);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(138, 26);
            this.btn6.TabIndex = 26;
            this.btn6.Text = "Run Nonce Get";
            this.btn6.UseVisualStyleBackColor = true;
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(415, 162);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(295, 42);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox
            // 
            this.textBox.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox.Location = new System.Drawing.Point(332, 16);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(84, 25);
            this.textBox.TabIndex = 7;
            this.textBox.Text = "COM?";
            // 
            // label_PortStatus
            // 
            this.label_PortStatus.AutoSize = true;
            this.label_PortStatus.Location = new System.Drawing.Point(217, 46);
            this.label_PortStatus.Name = "label_PortStatus";
            this.label_PortStatus.Size = new System.Drawing.Size(179, 15);
            this.label_PortStatus.TabIndex = 9;
            this.label_PortStatus.Text = "PortStatus : disconnected";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.Location = new System.Drawing.Point(217, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Input your port :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(34, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(377, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Enter the command which you want to send in the box :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(311, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "Enter Dst node (Hex) :";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(476, 131);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(57, 25);
            this.textBox2.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(42, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "2. Add mode";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(195, 308);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(57, 25);
            this.textBox3.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(31, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Enter Dst node (Hex) :";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(231, 340);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(479, 25);
            this.textBox4.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point(30, 345);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(195, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "Enter network Key (16byte) :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label7.Location = new System.Drawing.Point(30, 443);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(336, 15);
            this.label7.TabIndex = 19;
            this.label7.Text = "Enter Nonce Report As Soon As Possible (Hex) :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label8.Location = new System.Drawing.Point(30, 383);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(377, 15);
            this.label8.TabIndex = 22;
            this.label8.Text = "Enter the command which you want to send in the box :";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(415, 371);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox5.Size = new System.Drawing.Size(295, 42);
            this.textBox5.TabIndex = 23;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(375, 419);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(335, 66);
            this.textBox6.TabIndex = 24;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::WYP_ZWave_Spoofing_Tool.Properties.Resources.WYP_logo_new;
            this.pictureBox2.Location = new System.Drawing.Point(476, 16);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(225, 97);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WYP_ZWave_Spoofing_Tool.Properties.Resources.right;
            this.pictureBox1.Location = new System.Drawing.Point(256, 311);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 21);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // btn7
            // 
            this.btn7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn7.Location = new System.Drawing.Point(221, 491);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(145, 39);
            this.btn7.TabIndex = 28;
            this.btn7.Text = "6. Program Exit";
            this.btn7.UseVisualStyleBackColor = true;
            this.btn7.Click += new System.EventHandler(this.btn7_Click);
            // 
            // btn8
            // 
            this.btn8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn8.Location = new System.Drawing.Point(33, 491);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(139, 39);
            this.btn8.TabIndex = 29;
            this.btn8.Text = "5. Dos Attack";
            this.btn8.UseVisualStyleBackColor = true;
            this.btn8.Click += new System.EventHandler(this.btn8_Click);
            // 
            // MdiMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 553);
            this.Controls.Add(this.btn8);
            this.Controls.Add(this.btn7);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_PortStatus);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn6);
            this.Controls.Add(this.btn5);
            this.Controls.Add(this.btn4);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MdiMain";
            this.Padding = new System.Windows.Forms.Padding(30);
            this.Text = "WYP Z-Wave Spoofing Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label_PortStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn8;
    }
}

