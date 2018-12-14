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
    public partial class MdiMain : Form
    {
        public static string serialPortName = "COM3";
        public static ControllerStatus controllerStatus = ControllerStatus.Disconnected;
        public static bool showDebugOutput = false;
        ZWaveController controller = new ZWaveController(serialPortName);

        public static string dump_command = null;
        public static string dump_DST_nodeid = null;
        public static string dump_Network_Key = null;
        public static string dump_Nonce = null;
        public static byte[] nonce_get_command = new byte[2] { 0x98, 0x40 };
        byte[] real_dst_for_enc = new byte[1];

        public MdiMain()
        {
            InitializeComponent();
            //btn1.Click += Btn1_Click;
            
            // Register controller event handlers
            controller.ControllerStatusChanged += Controller_ControllerStatusChanged;
            controller.DiscoveryProgress += Controller_DiscoveryProgress;
            controller.NodeOperationProgress += Controller_NodeOperationProgress;
            controller.NodeUpdated += Controller_NodeUpdated;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 항목.속성명 = 값 (bool, 수치)
            // 항목.속성명 = 클래스.프로퍼티
            btn1.Cursor = Cursors.AppStarting;
        }

        // 1. Setup serial port
        public void Btn1_Click(object sender, EventArgs e)
        {
            string com_port = textBox.Text.Trim();
            SetSerialPortName(controller, com_port);
            //string com_port = textBox.Text.Trim();
            if (controller.PortName == com_port)
            {
                label_PortStatus.ForeColor = System.Drawing.Color.Red;
                label_PortStatus.Text = com_port + " connected";
            }
            
        }

        // 2. Add node start
        private void btn2_Click(object sender, EventArgs e)
        {
            StartNodeAdd(controller);
        }

        // 3. Add node stop
        private void btn3_Click(object sender, EventArgs e)
        {
            StopNodeAdd(controller);
        }

        // 4. Run command (non-security)
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
            StringToHex.String_to_Hex(buf1, real_command, buf1.Length);

            dump_DST_nodeid = null;
            dump_command = null;

            Send_command.RunCommand_Nonsecurity(controller, real_dst, real_command);
            
        }

        // Run Nonce Get
        private void btn6_Click(object sender, EventArgs e)
        {
            dump_DST_nodeid = textBox3.Text;
            dump_DST_nodeid = Regex.Replace(dump_DST_nodeid, " ", "");
            char[] buf = dump_DST_nodeid.ToCharArray();
            //byte[] real_dst = new byte[1];
            StringToHex.String_to_Hex(buf, real_dst_for_enc, buf.Length);
            Send_command.RunCommand_Nonsecurity(controller, real_dst_for_enc, nonce_get_command);
        }

        // 5. Run command (S0, S2)
        private void btn5_Click(object sender, EventArgs e)
        {
            /*
            dump_DST_nodeid = textBox3.Text;
            dump_DST_nodeid = Regex.Replace(dump_DST_nodeid, " ", "");
            char[] buf = dump_DST_nodeid.ToCharArray();
            byte[] real_dst = new byte[1];
            StringToHex.String_to_Hex(buf, real_dst, buf.Length);
            */

            dump_Network_Key = textBox4.Text;
            dump_Network_Key = Regex.Replace(dump_Network_Key, " ", "");
            char[] buf1 = dump_Network_Key.ToCharArray();
            byte[] real_Kn = new byte[16];
            StringToHex.String_to_Hex(buf1, real_Kn, buf1.Length);

            dump_command = textBox5.Text;
            dump_command = Regex.Replace(dump_command, " ", "");
            char[] buf2 = dump_command.ToCharArray();
            byte[] real_cm = new byte[buf2.Length / 2];
            StringToHex.String_to_Hex(buf2, real_cm, buf2.Length);
            
            dump_Nonce = textBox6.Text;
            dump_Nonce = Regex.Replace(dump_Nonce, " ", "");
            char[] buf3 = dump_Nonce.ToCharArray();
            byte[] real_nonce = new byte[8];
            StringToHex.String_to_Hex(buf3, real_nonce, buf3.Length);

            Send_command.RunCommand_Security(controller, real_dst_for_enc, real_cm, real_Kn, real_nonce);

            dump_DST_nodeid = null;
            real_dst_for_enc[0] = 0;
            dump_Network_Key = null;
            dump_command = null;
            dump_Nonce = null;

        }

        // 6. Run B company (S0)
        /*
        private void btn6_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form1"] is Form1 form1)
            {
                form1.Focus();
                return;
            }
            else
            {
                //Console.WriteLine("Hello, World!");
                form1 = new Form1(strParameter);
                form1.Show();
            }
        }
        */
        
        private void btn7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }


        // Dos Attack
        private void btn8_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form1"] is Form1 form1)
            {
                form1.Focus();
                return;
            }
            else
            {
                form1 = new Form1(controller);
                form1.Show();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private static void StartNodeAdd(ZWaveController controller)
        {
            controller.BeginNodeAdd();
        }

        private static void StopNodeAdd(ZWaveController controller)
        {
            controller.StopNodeAdd();
        }



        public static void SetSerialPortName(ZWaveController controller, string com_port)
        {
            var port = com_port;
            if (!String.IsNullOrWhiteSpace(port))
            {
                serialPortName = port;
                controller.PortName = serialPortName;
                controller.Connect();
            }
        }

        #region ZWaveController events handling

        private static void Controller_ControllerStatusChanged(object sender, ControllerStatusEventArgs args)
        {
            Console.WriteLine("ControllerStatusChange {0}", args.Status);
            var controller = (sender as ZWaveController);
            controllerStatus = args.Status;
            switch (controllerStatus)
            {
                case ControllerStatus.Connected:
                    // Initialize the controller and get the node list
                    controller.GetControllerInfo();
                    controller.GetControllerCapabilities();
                    controller.GetHomeId();
                    controller.GetSucNodeId();
                    controller.Initialize();
                    break;
                case ControllerStatus.Disconnected:
                    //ShowMenu();
                    break;
                case ControllerStatus.Initializing:
                    break;
                case ControllerStatus.Ready:
                    break;
                case ControllerStatus.Error:
                    Console.WriteLine("\nEnter [+] to try reconnect\n");
                    break;
            }
            //ToggleDebug(false);
        }

        private static void Controller_DiscoveryProgress(object sender, DiscoveryProgressEventArgs args)
        {
            Console.WriteLine("DiscoveryProgress {0}", args.Status);
            switch (args.Status)
            {
                case DiscoveryStatus.DiscoveryStart:
                    break;
                case DiscoveryStatus.DiscoveryEnd:
                    break;
            }
        }

        private static void Controller_NodeOperationProgress(object sender, NodeOperationProgressEventArgs args)
        {
            Console.WriteLine("NodeOperationProgress {0} {1}", args.NodeId, args.Status);
        }

        private static void Controller_NodeUpdated(object sender, NodeUpdatedEventArgs args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("NodeUpdated {0} Event Parameter {1} Value {2}", args.NodeId, args.Event.Parameter, args.Event.Value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion

        #region Utility Methods

        private static void ShowZWaveLibApi()
        {
            var zwavelib = Assembly.LoadFrom("ZWaveLib.dll");
            var typelist = zwavelib.GetTypes().ToList();
            typelist.Sort(new Comparison<Type>((a, b) => String.Compare(a.Name, b.Name)));
            foreach (var typeClass in typelist)
            {
                if (typeClass.FullName.StartsWith("ZWaveLib.CommandClasses"))
                {
                    var classMethods = typeClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
                    if (classMethods.Length > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n{0}", typeClass.Name);
                        Console.ForegroundColor = ConsoleColor.White;
                        foreach (var method in classMethods)
                        {
                            var parameters = method.GetParameters();
                            var parameterDescription = string.Join
                            (", ", parameters
                                .Select(x => /*x.ParameterType + " " +*/ x.Name)
                                .ToArray());

                            Console.WriteLine("{0} {1} ({2})", "  "/*method.ReturnType*/, method.Name, parameterDescription);
                        }
                    }
                }
            }
            Console.WriteLine("\n");
        }

        #endregion

    }
}
