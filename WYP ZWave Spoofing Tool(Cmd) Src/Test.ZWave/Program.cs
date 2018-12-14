/*
  This file is part of ZWaveLib (https://github.com/genielabs/zwave-lib-dotnet)

  Copyright (2012-2018) G-Labs (https://github.com/genielabs)

  Licensed under the Apache License, Version 2.0 (the "License");
  you may  not use this file except in compliance with the License.
  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

using NLog;
using NLog.Config;

using ZWaveLib;
using ZWaveLib.CommandClasses;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
// For GUI

namespace Test.ZWave
{
    internal class MainClass
    {
        //byte[] us_file_text = Enumerable.Repeat<byte>(0, 40).ToArray<byte>();
        public static byte[] Kn_LG = { 0xF6, 0xCD, 0xF5, 0xEC, 0xC8, 0x6A, 0xD2, 0x88, 0x83, 0xC8, 0x49, 0x1A, 0x8C, 0x02, 0xD0, 0x06 };
        public static byte[] Kn_KT = { 0x44, 0x6F, 0xBA, 0x73, 0xB0, 0x3A, 0xD7, 0x45, 0x69, 0x20, 0xF8, 0xD8, 0x0B, 0xC1, 0x27, 0x5D };
        public static byte[] Passwd_c = Enumerable.Repeat<byte>(0xaa, 16).ToArray<byte>();
        public static byte[] Passwd_m = Enumerable.Repeat<byte>(0x55, 16).ToArray<byte>();
        //public static byte[] IV = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
        public static byte[] IV = new byte[16] { 0x2D, 0xB0, 0xE3, 0x52, 0x86, 0x38, 0xB5, 0x0C, 0, 0, 0, 0, 0, 0, 0, 0};
        //AB 66 91 C3 58 67 BE 56
        public static byte SH = 0x81;
        public static byte SRC = 0x01;
        public static byte DST = 0x16;
        public static string Nonce_Get = null;
        public static byte[] Nonce_DoorLock_KT1 = Enumerable.Repeat<byte>(0, 8).ToArray<byte>(); // this is power bar off
        public static byte[] Nonce_DoorLock_KT = Enumerable.Repeat<byte>(0, 8).ToArray<byte>();  // open doorlock
        public static byte[] Nonce_DoorLock_KT2 = Enumerable.Repeat<byte>(0, 8).ToArray<byte>(); // this is power bar on
        public static byte[] Nonce_DoorLock_KT3 = Enumerable.Repeat<byte>(0, 8).ToArray<byte>(); // DoorLock Password Set
        public static string plain = null;

        private static string serialPortName = "COM3";
        private static ControllerStatus controllerStatus = ControllerStatus.Disconnected;
        private static bool showDebugOutput = false;
        private static readonly LoggingRule LoggingRule = LogManager.Configuration.LoggingRules[0];

        public static void Main(string[] cargs)
        {
            //Application.EnableVisualStyles(); // For GUI
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MdiMain());
            

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nZWaveLib Test Program\n");
            Console.ForegroundColor = ConsoleColor.White;

            var controller = new ZWaveController(serialPortName);
            // Register controller event handlers
            controller.ControllerStatusChanged += Controller_ControllerStatusChanged;;
            controller.DiscoveryProgress += Controller_DiscoveryProgress;
            controller.NodeOperationProgress += Controller_NodeOperationProgress;
            controller.NodeUpdated += Controller_NodeUpdated;

            // Main program loop
            var command = "";
            while (command != "!")
            {
                ShowMenu();
                // TODO: Allow issuing CommandClass commands on nodes from the console input
                // TODO: Add "Associate node to controller" option
                // TODO: Add "Query node parameters" based on implemented classes
                command = Console.ReadLine();
                switch (command)
                {
                    case "0":
                        ToggleDebug(!showDebugOutput);
                        break;
                    case "1":
                        ListNodes(controller);
                        break;
                    case "2":
                        StartNodeAdd(controller);
                        break;
                    case "3":
                        StopNodeAdd(controller);
                        break;
                    case "4":
                        StartNodeRemove(controller);
                        break;
                    case "5":
                        StopNodeRemove(controller);
                        break;
                    case "6":
                        HealNetwork(controller);
                        break;
                    case "7":
                        RunStressTest(controller);
                        break;
                    case "8":
                        ShowZWaveLibApi();
                        break;
                    case "9":
                        Discovery(controller);
                        break;
                    case "?":
                        SetSerialPortName(controller);
                        break;
                    case "+":
                        controller.Connect();
                        break;
                    case "~":
                        RunCommandInteractive(controller);
                        break;
                    case "10":
                        Console.WriteLine("[1] Open DoorLock \n");
                        Console.WriteLine("[2] Power Bar Off \n");
                        Console.WriteLine("[3] Power Bar On \n");
                        Console.WriteLine("[4] DoorLock Password Set \n");
                        Console.WriteLine("[5] Quit ");
                        Console.WriteLine("------------------------------\n");
                        Console.WriteLine("------------------------------\n");
                        var select1 = Console.ReadLine();
                        switch (select1)
                        {
                            case "1": // open doorlock
                                //Console.WriteLine("1. Input Dst Node ID");
                                //byte LGDstNodeId1 = (byte)Console.Read();
                                //RunCommand_FNR(controller, LGDstNodeId1); 
                                /*
                                Console.WriteLine("Input Your Command class");
                                plain = Console.ReadLine();
                                plain = Regex.Replace(plain, " ", "");
                                char[] p = plain.ToCharArray();
                                byte[] plain1 = null;
                                plain1 = String_to_Hex(p, plain1, p.Length);
                                */
                                
                                //RunCommand_FNR(controller);
                                RunCommand_NONCE_GET(controller);
                                
                                Console.WriteLine("Input Your Nonce ");
                                Nonce_Get = Console.ReadLine();
                                Nonce_Get = Regex.Replace(Nonce_Get, " ", "");
                                char[] buf = Nonce_Get.ToCharArray();
                                String_to_Hex(buf, Nonce_DoorLock_KT, buf.Length);
                                //Nonce_DoorLock_KT = String_to_Hex(buf, Nonce_DoorLock_KT, buf.Length);
                                Console.WriteLine("Your Input data : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X}", Nonce_DoorLock_KT[0], Nonce_DoorLock_KT[1], Nonce_DoorLock_KT[2], Nonce_DoorLock_KT[3], Nonce_DoorLock_KT[4], Nonce_DoorLock_KT[5], Nonce_DoorLock_KT[6], Nonce_DoorLock_KT[7]);
                                Nonce_Get = null;
                                RunCommand_DoorLock_OPEN_KT(controller);


                                break;
                            case "2": // Power Bar Off
                                //Console.WriteLine("1. Input Dst Node ID");
                                //byte LGDstNodeId2 = (byte)Console.Read();
                                RunCommand_NONCE_GET_P(controller);
                                Nonce_Get = Console.ReadLine();
                                Nonce_Get = Regex.Replace(Nonce_Get, " ", "");
                                char[] buf1 = Nonce_Get.ToCharArray();
                                String_to_Hex(buf1, Nonce_DoorLock_KT1, buf1.Length); // this is not DoorLock
                                Console.WriteLine("Your Input data : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X}", Nonce_DoorLock_KT1[0], Nonce_DoorLock_KT1[1], Nonce_DoorLock_KT1[2], Nonce_DoorLock_KT1[3], Nonce_DoorLock_KT1[4], Nonce_DoorLock_KT1[5], Nonce_DoorLock_KT1[6], Nonce_DoorLock_KT1[7]);
                                Nonce_Get = null;
                                RunCommand_DoorLock_OPEN_KT_P_Off(controller);


                                break;
                            case "3": // Power Bar On
                                RunCommand_NONCE_GET_P(controller);
                                Nonce_Get = Console.ReadLine();
                                Nonce_Get = Regex.Replace(Nonce_Get, " ", "");
                                char[] buf2 = Nonce_Get.ToCharArray();
                                String_to_Hex(buf2, Nonce_DoorLock_KT2, buf2.Length);
                                Console.WriteLine("Your Input data : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X}", Nonce_DoorLock_KT2[0], Nonce_DoorLock_KT2[1], Nonce_DoorLock_KT2[2], Nonce_DoorLock_KT2[3], Nonce_DoorLock_KT2[4], Nonce_DoorLock_KT2[5], Nonce_DoorLock_KT2[6], Nonce_DoorLock_KT2[7]);
                                Nonce_Get = null;
                                RunCommand_DoorLock_OPEN_KT_P_On(controller);

                                break;
                            case "4": // DoorLock Password Set
                                RunCommand_NONCE_GET(controller);
                                Nonce_Get = Console.ReadLine();
                                Nonce_Get = Regex.Replace(Nonce_Get, " ", "");
                                char[] buf3 = Nonce_Get.ToCharArray();
                                String_to_Hex(buf3, Nonce_DoorLock_KT3, buf3.Length);
                                Console.WriteLine("Your Input data : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X}", Nonce_DoorLock_KT3[0], Nonce_DoorLock_KT3[1], Nonce_DoorLock_KT3[2], Nonce_DoorLock_KT3[3], Nonce_DoorLock_KT3[4], Nonce_DoorLock_KT3[5], Nonce_DoorLock_KT3[6], Nonce_DoorLock_KT3[7]);
                                Nonce_Get = null;
                                RunCommand_DoorLock_OPEN_KT_pass_set(controller);

                                break;
                            case "5": // quit
                                break;
                        }

                        /*
                         * Not Implemented Yet.
                         * 1. Send Find Node Range 01 04 02 1F 01 03 .
                         * 2. 98 04 --> When this packet is S0.
                        */
                        //var LGnonce = Console.ReadLine();
                        // Then, have to implement seding packet which contain total encrypted data.

                        break;
                }
            }
            Console.WriteLine("\nExit!\n");
            controller.Dispose();
        }
        

        private static void RunCommandInteractive(ZWaveController controller)
        {
            //Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
            //Console.Write("> ");
            
            //var command = Console.ReadLine();
            //var command = "DoorLock, Set, 2, 5";
            var command = "WakeUp, WakeUpNode, 2";

            if(string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] {'.', '(', ')', ',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if(!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node = controller.GetNode(nodeId);
                //node.Id = 0x0a;

                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;

                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node };
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }

                methodToInvoke.Invoke(null, invokeParams.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }


        //send find_node_range (FNR) packet 
        private static void RunCommand_FNR(ZWaveController controller)
        {
            //Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
            //Console.Write("> ");

            //var command = Console.ReadLine();
            var command = "DoorLock, Set1, 2, 5";
            if (string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] { '.', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if (!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node1 = controller.GetNode(nodeId);
                node1.Id = 0x10;
                //node1.Id = 31;
                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;

                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node1 };
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }
                //byte[] request = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                //DoorLock.Set1(node, 0, request);
                methodToInvoke.Invoke(null, invokeParams.ToArray());

                node1.Id = 0x2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //send find_node_range (FNR) packet 
        private static void RunCommand_NONCE_GET(ZWaveController controller)
        {
            //Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
            //Console.Write("> ");

            //var command = Console.ReadLine();
            var command = "DoorLock, Set2, 2, 5";
            if (string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] { '.', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if (!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node2 = controller.GetNode(nodeId);
                //node2.Id = 0x16;
                
                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;
                
                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node2 };
                //byte[] test = Enumerable.Repeat<byte>(0, 8).ToArray<byte>(); // revised - 181106.17.42
                //invokeParams.Add(test); // revised
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }
                //byte[] request = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                //DoorLock.Set1(node, 0, request);

                //invokeParams.Add(test);
                node2.Id = 0x10;
                //node2.Id = 31;

                node2.SendDataRequest(new byte[] {
                (byte)0x98,
                (byte)0x40
                });
                node2.Id = 0x02;
                //methodToInvoke.Invoke(null, invokeParams.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void RunCommand_DoorLock_OPEN_KT(ZWaveController controller)
        {
            var command = "DoorLock, Set2, 2, 5";
            if (string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] { '.', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if (!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node3 = controller.GetNode(nodeId);
                //node3.Id = 0x16; 

                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;

                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node3 };
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }

                byte[] DoorLock_Open_MAC = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] Header_KT_DoorLock = Enumerable.Repeat<byte>(0, 20).ToArray<byte>();
                byte[] DoorLock_Open_Kc = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] DoorLock_Open_Km = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] X1 = { 0x00, 0x62, 0x01, 0x00 }; // DoorLock open command
                //byte[] X1 = { 0x00, 0x25, 0x01, 0x00 }; // DoorLock open command
                byte[] DoorLock_Open_C = Enumerable.Repeat<byte>(0, 4).ToArray<byte>();
                DoorLock_Open_Kc = AES128_ECB_ENC(Passwd_c, Kn_KT);
                DoorLock_Open_Km = AES128_ECB_ENC(Passwd_m, Kn_KT);
                for (int i = 8; i < 16; i++) IV[i] = Nonce_DoorLock_KT[i-8];
                DoorLock_Open_C = AES128_OFB_ENC(X1, DoorLock_Open_Kc, IV);
                for (int i=0; i<16; i++) { Header_KT_DoorLock[i] = IV[i]; }
                Header_KT_DoorLock[16] = SH;
                Header_KT_DoorLock[17] = SRC;
                //Header_KT_DoorLock[17] = 0x1;
                Header_KT_DoorLock[18] = 0x10;  // It is DST
                //Header_KT_DoorLock[18] = 31;  // It is DST
                Header_KT_DoorLock[19] = 0x04;
                DoorLock_Open_MAC = AES128_CBCMAC_ENC(Header_KT_DoorLock, DoorLock_Open_C, DoorLock_Open_Km);
                
                int packet_length = 23;
                byte[] S_P = Enumerable.Repeat<byte>(0, packet_length).ToArray<byte>();
                S_P[0] = 0x98;  S_P[1] = 0x81;
                for(int i=0; i<8; i++) { S_P[i+2] = IV[i]; }
                for(int i=0; i<4; i++) { S_P[i+10] = DoorLock_Open_C[i]; }
                S_P[14] = IV[8]; 
                for(int i=0; i<8; i++) { S_P[15 + i] = DoorLock_Open_MAC[i]; }


                node3.Id = 0x10; // revised
                //node3.Id = 31;
                node3.SendDataRequest(S_P);
                node3.Id = 0x02;
                /*
                node.SendDataRequest(new byte[] {
                (byte)0x98,
                (byte)0x40
                });
                */
                //methodToInvoke.Invoke(null, invokeParams.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void RunCommand_NONCE_GET_P(ZWaveController controller)
        {
            //Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
            //Console.Write("> ");

            //var command = Console.ReadLine();
            var command = "DoorLock, Set2, 2, 5";
            if (string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] { '.', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if (!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node2 = controller.GetNode(nodeId);
                //node2.Id = 0x16;

                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;
                
                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node2 };
                //byte[] test = Enumerable.Repeat<byte>(0, 8).ToArray<byte>(); // revised - 181106.17.42
                //invokeParams.Add(test); // revised
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }
                //byte[] request = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                //DoorLock.Set1(node, 0, request);

                //invokeParams.Add(test);
                //node2.Id = 0x10;
                node2.Id = 31;

                //WakeUp.WakeUpNode(node2);   // have to find more
                node2.SendDataRequest(new byte[] {
                (byte)0x98,
                (byte)0x40
                });
                node2.Id = 0x02;
                //methodToInvoke.Invoke(null, invokeParams.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void RunCommand_DoorLock_OPEN_KT_P_Off(ZWaveController controller)
        {
            var command = "DoorLock, Set2, 2, 5";
            if (string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] { '.', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if (!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node3 = controller.GetNode(nodeId);
                //node3.Id = 0x16; 

                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;

                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node3 };
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }

                byte[] DoorLock_Open_MAC = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] Header_KT_DoorLock = Enumerable.Repeat<byte>(0, 20).ToArray<byte>();
                byte[] DoorLock_Open_Kc = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] DoorLock_Open_Km = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                //byte[] X1 = { 0x00, 0x62, 0x01, 0x00 }; // DoorLock open command
                byte[] X1 = { 0x00, 0x25, 0x01, 0x00 }; // Power Bar Off
                byte[] DoorLock_Open_C = Enumerable.Repeat<byte>(0, 4).ToArray<byte>();
                DoorLock_Open_Kc = AES128_ECB_ENC(Passwd_c, Kn_KT);
                DoorLock_Open_Km = AES128_ECB_ENC(Passwd_m, Kn_KT);
                for (int i = 8; i < 16; i++) IV[i] = Nonce_DoorLock_KT1[i - 8];
                DoorLock_Open_C = AES128_OFB_ENC(X1, DoorLock_Open_Kc, IV);
                for (int i = 0; i < 16; i++) { Header_KT_DoorLock[i] = IV[i]; }
                Header_KT_DoorLock[16] = SH;
                Header_KT_DoorLock[17] = SRC;
                //Header_KT_DoorLock[17] = 0x1;
                //Header_KT_DoorLock[18] = 0x10;  // It is DST
                Header_KT_DoorLock[18] = 31;  // It is DST
                Header_KT_DoorLock[19] = 0x04;
                DoorLock_Open_MAC = AES128_CBCMAC_ENC(Header_KT_DoorLock, DoorLock_Open_C, DoorLock_Open_Km);

                int packet_length = 23;
                byte[] S_P = Enumerable.Repeat<byte>(0, packet_length).ToArray<byte>();
                S_P[0] = 0x98; S_P[1] = 0x81;
                for (int i = 0; i < 8; i++) { S_P[i + 2] = IV[i]; }
                for (int i = 0; i < 4; i++) { S_P[i + 10] = DoorLock_Open_C[i]; }
                S_P[14] = IV[8];
                for (int i = 0; i < 8; i++) { S_P[15 + i] = DoorLock_Open_MAC[i]; }


                //node3.Id = 0x10; // revised
                node3.Id = 31;
                node3.SendDataRequest(S_P);
                node3.Id = 0x02;
                /*
                node.SendDataRequest(new byte[] {
                (byte)0x98,
                (byte)0x40
                });
                */
                //methodToInvoke.Invoke(null, invokeParams.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        private static void RunCommand_DoorLock_OPEN_KT_P_On(ZWaveController controller)
        {
            var command = "DoorLock, Set2, 2, 5";
            if (string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] { '.', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if (!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node4 = controller.GetNode(nodeId);
                //node3.Id = 0x16; 

                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;

                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node4 };
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }

                byte[] DoorLock_Open_MAC = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] Header_KT_DoorLock = Enumerable.Repeat<byte>(0, 20).ToArray<byte>();
                byte[] DoorLock_Open_Kc = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] DoorLock_Open_Km = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                //byte[] X1 = { 0x00, 0x62, 0x01, 0x00 }; // DoorLock open command
                byte[] X1 = { 0x00, 0x25, 0x01, 0xFF }; // Power Bar On
                byte[] DoorLock_Open_C = Enumerable.Repeat<byte>(0, 4).ToArray<byte>();
                DoorLock_Open_Kc = AES128_ECB_ENC(Passwd_c, Kn_KT);
                DoorLock_Open_Km = AES128_ECB_ENC(Passwd_m, Kn_KT);
                for (int i = 8; i < 16; i++) IV[i] = Nonce_DoorLock_KT2[i - 8];
                DoorLock_Open_C = AES128_OFB_ENC(X1, DoorLock_Open_Kc, IV);
                for (int i = 0; i < 16; i++) { Header_KT_DoorLock[i] = IV[i]; }
                Header_KT_DoorLock[16] = SH;
                Header_KT_DoorLock[17] = SRC;
                //Header_KT_DoorLock[17] = 0x1;
                //Header_KT_DoorLock[18] = 0x10;  // It is DST
                Header_KT_DoorLock[18] = 31;  // It is DST
                Header_KT_DoorLock[19] = 0x04;
                DoorLock_Open_MAC = AES128_CBCMAC_ENC(Header_KT_DoorLock, DoorLock_Open_C, DoorLock_Open_Km);

                int packet_length = 23;
                byte[] S_P = Enumerable.Repeat<byte>(0, packet_length).ToArray<byte>();
                S_P[0] = 0x98; S_P[1] = 0x81;
                for (int i = 0; i < 8; i++) { S_P[i + 2] = IV[i]; }
                for (int i = 0; i < 4; i++) { S_P[i + 10] = DoorLock_Open_C[i]; }
                S_P[14] = IV[8];
                for (int i = 0; i < 8; i++) { S_P[15 + i] = DoorLock_Open_MAC[i]; }


                //node3.Id = 0x10; // revised
                node4.Id = 31;
                node4.SendDataRequest(S_P);
                node4.Id = 0x02;
                /*
                node.SendDataRequest(new byte[] {
                (byte)0x98,
                (byte)0x40
                });
                */
                //methodToInvoke.Invoke(null, invokeParams.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void RunCommand_DoorLock_OPEN_KT_pass_set(ZWaveController controller)
        {
            var command = "DoorLock, Set2, 2, 5";
            if (string.IsNullOrEmpty(command))
                return;

            var commandTerms = command.Split(new[] { '.', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandTerms.Length < 3)
            {
                Console.WriteLine("Commands must be issued in format CommandClass.Command(nodeId, _additional params_).");
                return;
            }

            byte nodeId;
            if (!byte.TryParse(commandTerms[2], out nodeId))
                return;

            try
            {
                var node5 = controller.GetNode(nodeId);
                //node3.Id = 0x16; 

                var ccType = Assembly.GetAssembly(typeof(ZWaveController)).GetType(string.Format("ZWaveLib.CommandClasses.{0}", commandTerms[0]), true);
                if (ccType == null)
                    return;

                // currently we try to find method using it's name and parameters count
                var methodInfos = ccType.GetMethods();
                MethodInfo methodToInvoke = null;
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.Name == commandTerms[1] && methodInfo.GetParameters().Length == commandTerms.Length - 2)
                        methodToInvoke = methodInfo;
                }
                if (methodToInvoke == null)
                    return;

                // prepare params
                const int additionalParamsIdx = 3;
                var invokeParams = new List<object> { node5 };
                var methodParams = methodToInvoke.GetParameters();
                for (var i = 0; i < commandTerms.Length - 3; i++)
                {
                    var paramType = methodParams[i + 1].ParameterType;
                    var val = TypeDescriptor.GetConverter(paramType).ConvertFromString(commandTerms[additionalParamsIdx + i]);
                    invokeParams.Add(val);
                }

                byte[] DoorLock_Open_MAC = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] Header_KT_DoorLock = Enumerable.Repeat<byte>(0, 20).ToArray<byte>();
                byte[] DoorLock_Open_Kc = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] DoorLock_Open_Km = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                //byte[] X1 = { 0x00, 0x62, 0x01, 0x00 }; // DoorLock open command
                //byte[] X1 = { 0x00, 0x25, 0x01, 0x00 }; // DoorLock open command
                byte[] X1 = { 0x00, 0x63, 0x01, 0x03, 0x01, 0x35, 0x35, 0x35, 0x35 }; // DoorLock Set Pass : 9byte 
                byte[] DoorLock_Open_C = Enumerable.Repeat<byte>(0, 9).ToArray<byte>();
                DoorLock_Open_Kc = AES128_ECB_ENC(Passwd_c, Kn_KT);
                DoorLock_Open_Km = AES128_ECB_ENC(Passwd_m, Kn_KT);
                for (int i = 8; i < 16; i++) IV[i] = Nonce_DoorLock_KT3[i - 8];
                DoorLock_Open_C = AES128_OFB_ENC_9(X1, DoorLock_Open_Kc, IV);
                for (int i = 0; i < 16; i++) { Header_KT_DoorLock[i] = IV[i]; }
                Header_KT_DoorLock[16] = SH;
                Header_KT_DoorLock[17] = SRC;
                //Header_KT_DoorLock[17] = 0x1;
                Header_KT_DoorLock[18] = 0x10;  // It is DST
                //Header_KT_DoorLock[18] = 31;  // It is DST
                Header_KT_DoorLock[19] = 0x09;
                DoorLock_Open_MAC = AES128_CBCMAC_ENC(Header_KT_DoorLock, DoorLock_Open_C, DoorLock_Open_Km);

                int packet_length = 28;
                byte[] S_P = Enumerable.Repeat<byte>(0, packet_length).ToArray<byte>();
                S_P[0] = 0x98; S_P[1] = 0x81;
                for (int i = 0; i < 8; i++) { S_P[i + 2] = IV[i]; }
                for (int i = 0; i < 9; i++) { S_P[i + 10] = DoorLock_Open_C[i]; }
                S_P[19] = IV[8];
                for (int i = 0; i < 8; i++) { S_P[20 + i] = DoorLock_Open_MAC[i]; }


                node5.Id = 0x10; // revised
                //node3.Id = 31;
                node5.SendDataRequest(S_P);
                node5.Id = 0x02;
                /*
                node.SendDataRequest(new byte[] {
                (byte)0x98,
                (byte)0x40
                });
                */
                //methodToInvoke.Invoke(null, invokeParams.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        public static void String_to_Hex(char[] test, byte[] target, int len)
        {
            char ch;
            byte[] us_file_text = Enumerable.Repeat<byte>(0, 40).ToArray<byte>();
            
            for(int i=0; i<len; i++)
            {
                ch = test[i];
                if (ch >= '0' && ch <= '9') us_file_text[i] = (byte)(ch - '0');
                else if (ch >= 'a' && ch <= 'f') us_file_text[i] = (byte)(ch - 'a' + 0xa);
                else if (ch >= 'A' && ch <= 'F') us_file_text[i] = (byte)(ch - 'A' + 0xa);
            }

            if ((len % 2) == 1)
            {
                for (int i = 0; i < len - 1; i += 2)
                {
                    target[i / 2] = (byte)((us_file_text[i] << 4) ^ (us_file_text[i + 1]));
                }
                target[len / 2] = (byte)(0x0F & us_file_text[len - 1]);
            }
            else
            {
                for (int i = 0; i < len; i += 2)
                {
                    target[i / 2] = (byte)((us_file_text[i] << 4) ^ us_file_text[i + 1]);
                }
            }

            //return target;
        }
        
        public static byte[] AES128_CBC_ENC(byte[] textToEncrypt, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[16];
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            //Console.WriteLine("Key : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X} {8:X} {9:X} {10:X} {11:X} {12:X} {13:X} {14:X} {15:X} ", key[0], key[1], key[2], key[3], key[4], key[5], key[6], key[7], key[8], key[9], key[10], key[11], key[12], key[13], key[14], key[15]);
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = IV;
            byte[] encText = rijndaelCipher.CreateEncryptor().TransformFinalBlock(textToEncrypt, 0, textToEncrypt.Length);

            //return encText;
            for (int i = 0; i < 16; i++) { return_value[i] = encText[i]; }
            return return_value;
        }

        public static byte[] AES128_CBC_DEC(byte[] textToDecrypt, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[16];
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            //Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = IV;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(textToDecrypt, 0, textToDecrypt.Length); // have to revise

            //return plainText; // have to revise
            for(int i=0; i<16; i++) { return_value[i] = plainText[i]; }
            return return_value;
        }

        public static byte[] AES128_ECB_ENC(byte[] input, byte[] key)
        {
            byte[] return_value = new byte[16];
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.ECB;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            rijndaelCipher.Key = key;
            byte[] encText = rijndaelCipher.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);

            for (int i = 0; i < 16; i++) { return_value[i] = encText[i]; }
            return return_value;
        }

        public static byte[] AES128_OFB_ENC(byte[] data, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[4];
            byte[] tempIV;
            tempIV = IV;

            int cipherIndex;
            int blockIndex = 0;
            byte[] plaintext16ByteChunk = new byte[16];

            /*
            Console.WriteLine("Input Your Commnad ");
            string input = Console.ReadLine();
            input = Regex.Replace(input, " ", "");
            char[] buf = Nonce_Get.ToCharArray();
            int L = buf.Length;
            byte[] data = new byte[L];
            String_to_Hex(buf, data, buf.Length);
            Console.WriteLine("Your Input data : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X}", Nonce_DoorLock_KT[0], Nonce_DoorLock_KT[1], Nonce_DoorLock_KT[2], Nonce_DoorLock_KT[3], Nonce_DoorLock_KT[4], Nonce_DoorLock_KT[5], Nonce_DoorLock_KT[6], Nonce_DoorLock_KT[7]);
            */



            for (cipherIndex = 0; cipherIndex < data.Length; cipherIndex++)
            {
                plaintext16ByteChunk[blockIndex] = data[cipherIndex];
                blockIndex++;
                if (blockIndex == 16)
                {
                    tempIV = AES128_ECB_ENC(tempIV, key);
                    int ivIndex = 0;
                    for (int i = (cipherIndex - 15); i <= cipherIndex; i++)
                    {
                        data[i] = (byte)(plaintext16ByteChunk[ivIndex] ^ tempIV[ivIndex]);
                        ivIndex++;
                    }
                    plaintext16ByteChunk = new byte[16];
                    blockIndex = 0;
                }
            }

            if (blockIndex != 0)
            {
                tempIV = AES128_ECB_ENC(tempIV, key);
                //AES128_ECB_ENC(tempIV, key, tempIV);
                int ivIndex = 0;
                for (int i = 0; i < blockIndex; i++)
                {
                    data[cipherIndex - blockIndex + i] = (byte)(plaintext16ByteChunk[i] ^ tempIV[i]);
                    ivIndex++;
                }
            }
            
            for (int i = 0; i < return_value.Length; i++) { return_value[i] = data[i]; }
            //return data;
            return return_value;
        }

        public static byte[] AES128_OFB_ENC_9(byte[] data, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[9];
            byte[] tempIV;
            tempIV = IV;

            int cipherIndex;
            int blockIndex = 0;
            byte[] plaintext16ByteChunk = new byte[16];
            
            for (cipherIndex = 0; cipherIndex < data.Length; cipherIndex++)
            {
                plaintext16ByteChunk[blockIndex] = data[cipherIndex];
                blockIndex++;
                if (blockIndex == 16)
                {
                    tempIV = AES128_ECB_ENC(tempIV, key);
                    int ivIndex = 0;
                    for (int i = (cipherIndex - 15); i <= cipherIndex; i++)
                    {
                        data[i] = (byte)(plaintext16ByteChunk[ivIndex] ^ tempIV[ivIndex]);
                        ivIndex++;
                    }
                    plaintext16ByteChunk = new byte[16];
                    blockIndex = 0;
                }
            }

            if (blockIndex != 0)
            {
                tempIV = AES128_ECB_ENC(tempIV, key);
                //AES128_ECB_ENC(tempIV, key, tempIV);
                int ivIndex = 0;
                for (int i = 0; i < blockIndex; i++)
                {
                    data[cipherIndex - blockIndex + i] = (byte)(plaintext16ByteChunk[i] ^ tempIV[i]);
                    ivIndex++;
                }
            }

            for (int i = 0; i < return_value.Length; i++) { return_value[i] = data[i]; }
            //return data;
            return return_value;
        }

        public static byte[] AES128_CBCMAC_ENC(byte[] header, byte[] data, byte[] Km)
        {
            byte[] return_value = new byte[8];
            byte[] input16Byte = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
            byte[] MAC = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
            byte[] input = new byte[20 + data.Length];

            for (int i = 0; i < 20; i++)
            {
                input[i] = header[i];
            }
            for (int i = 0; i < data.Length; i++)
            {
                input[20 + i] = data[i];
            }
            for (int i = 0; i < 16; i++)
            {
                if (i >= input.Length)
                    input16Byte[i] = 0;
                else
                    input16Byte[i] = input[i];
            }
            MAC = AES128_ECB_ENC(input16Byte, Km);
            System.Array.Clear(input16Byte, 0, 16); 

            int cipherIndex;
            int blockIndex = 0;

            for (cipherIndex = 16; cipherIndex < input.Length; cipherIndex++)
            {
                input16Byte[blockIndex] = input[cipherIndex];
                blockIndex++;

                if (blockIndex == 16)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        MAC[i] = (byte)(input16Byte[i] ^ MAC[i]);
                    }
                    System.Array.Clear(input16Byte, 0, 16);
                    blockIndex = 0;

                    MAC = AES128_ECB_ENC(MAC, Km);
                }
            }
            if (blockIndex != 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    MAC[i] = (byte)(input16Byte[i] ^ MAC[i]);
                }
                MAC = AES128_ECB_ENC(MAC, Km);
            }

            //return MAC;
            for(int i=0; i<8; i++) { return_value[i] = MAC[i]; }
            return return_value;
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n[0] Toggle show debug (ShowDebug={0})", showDebugOutput);
            Console.WriteLine("[1] List nodes");
            Console.WriteLine("[2] Add node start");
            Console.WriteLine("[3] Add node stop");
            Console.WriteLine("[4] Remove node start");
            Console.WriteLine("[5] Remove node stop");
            Console.WriteLine("[6] Heal Network");
            Console.WriteLine("[7] Run Node Stress Test");
            Console.WriteLine("[8] Dump available ZWaveLib API commands");
            Console.WriteLine("[9] Discovery (query all nodes data)");
            Console.WriteLine("[?] Change serial port (PortName={0})", serialPortName);
            Console.WriteLine("[+] Connect / Reconnect (Status={0})", controllerStatus);
            Console.WriteLine("[~] Run command");
            Console.WriteLine("[!] Exit");
            Console.WriteLine("[10] A Company");
            Console.WriteLine("[11] B Company");
            Console.WriteLine("\nEnter option and hit [enter]:");
        }

        private static void ToggleDebug(bool show = false)
        {
            LogManager.Configuration.LoggingRules.Remove(LoggingRule);
            LogManager.Configuration.Reload();
            showDebugOutput = show;
            if (showDebugOutput)
            {
                LogManager.Configuration.LoggingRules.Add(LoggingRule);
                LogManager.Configuration.Reload();
            }
        }

        private static void ListNodes(ZWaveController controller)
        {
            foreach (var node in controller.Nodes)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nNode {0}", node.Id);
                Console.ForegroundColor = ConsoleColor.White;
                var mspecs = node.ManufacturerSpecific;
                Console.WriteLine("    Manufacturer Specific {0}-{1}-{2}", mspecs.ManufacturerId, mspecs.TypeId, mspecs.ProductId);
                Console.WriteLine("    Basic Type {0}", (GenericType)node.ProtocolInfo.BasicType);
                Console.WriteLine("    Generic Type {0}", (GenericType)node.ProtocolInfo.GenericType);
                Console.WriteLine("    Specific Type {0}", node.ProtocolInfo.SpecificType);
                Console.WriteLine("    Secure Info Frame {0}", BitConverter.ToString(node.SecuredNodeInformationFrame));
                Console.WriteLine("    Info Frame {0}", BitConverter.ToString(node.NodeInformationFrame));
                foreach (var nodeCmdClass in node.CommandClasses)
                {
                    var versionInfo = "";
                    // TODO: GetCmdClassVersion version is not currently working
                    if (node.SupportCommandClass(CommandClass.Version))
                    {
                        versionInfo = String.Format("(version {0})", nodeCmdClass.Version);
                    }
                    if (!Enum.IsDefined(typeof(CommandClass), nodeCmdClass.Id))
                    {
                        versionInfo += " [UNSUPPORTED]";
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("        {0} {1}", nodeCmdClass.CommandClass, versionInfo);
                }
                Console.ForegroundColor = ConsoleColor.White;
                if (node.Version != null)
                {
                    Console.WriteLine("    Node Version info:");
                    Console.WriteLine("        LibraryType {0}", (node.Version.LibraryType));
                    Console.WriteLine("        ProtocolVersion {0}", (node.Version.ProtocolVersion));
                    Console.WriteLine("        ProtocolSubVersion {0}", (node.Version.ProtocolSubVersion));
                    Console.WriteLine("        ApplicationVersion {0}", (node.Version.ApplicationVersion));
                    Console.WriteLine("        ApplicationSubVersion {0}", (node.Version.ApplicationSubVersion));
                }
                if (node.GetData("RoutingInfo") != null)
                {
                    Console.WriteLine("    Routing Info {0}", BitConverter.ToString((byte[])node.GetData("RoutingInfo").Value));
                }
            }
            Console.WriteLine("\n");
        }

        private static void StartNodeAdd(ZWaveController controller)
        {
            ToggleDebug(true);
            controller.BeginNodeAdd();
        }

        private static void StopNodeAdd(ZWaveController controller)
        {
            ToggleDebug(true);
            controller.StopNodeAdd();
            ToggleDebug(false);
        }

        private static void StartNodeRemove(ZWaveController controller)
        {
            ToggleDebug(true);
            controller.BeginNodeRemove();
        }

        private static void StopNodeRemove(ZWaveController controller)
        {
            ToggleDebug(true);
            controller.StopNodeRemove();
            ToggleDebug(false);
        }

        private static void HealNetwork(ZWaveController controller)
        {
            ToggleDebug(true);
            foreach (var node in controller.Nodes)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nHealing Node {0}", node.Id);
                Console.ForegroundColor = ConsoleColor.White;
                controller.RequestNeighborsUpdateOptions(node.Id);
                controller.RequestNeighborsUpdate(node.Id);
                controller.GetNeighborsRoutingInfo(node.Id);
            }
            ToggleDebug(false);
        }

        private static void RunStressTest(ZWaveController controller)
        {
            ToggleDebug(true);
            // loop 10 times
            for (var x = 0; x < 10; x++)
            {
                foreach (var node in controller.Nodes)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nNode {0} Controller.GetNodeInformationFrame", node.Id);
                    Console.ForegroundColor = ConsoleColor.White;
                    controller.GetNodeInformationFrame(node.Id);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nNode {0} Controller.GetNeighborsRoutingInfo", node.Id);
                    Console.ForegroundColor = ConsoleColor.White;
                    controller.GetNeighborsRoutingInfo(node.Id);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nNode {0} CommandClass.ManufacturerSpecific.Get", node.Id);
                    Console.ForegroundColor = ConsoleColor.White;
                    ManufacturerSpecific.Get(node);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nNode {0} CommandClass.Basic.Get", node.Id);
                    Console.ForegroundColor = ConsoleColor.White;
                    Basic.Get(node);
                }
                // Pause 2 secods between each test pass
                Thread.Sleep(2000);
            }
            ToggleDebug(false);
        }

        private static void Discovery(ZWaveController controller)
        {
            ToggleDebug(true);
            controller.Discovery();
            ToggleDebug(false);
        }

        private static void SetSerialPortName(ZWaveController controller)
        {
            Console.WriteLine("Enter the serial port name (eg. COM7 or /dev/ttyUSB0):");
            var port = Console.ReadLine().Trim();
            if (!String.IsNullOrWhiteSpace(port))
            {
                serialPortName = port;
                controller.PortName = serialPortName;
                controller.Connect();
            }
        }

        #region ZWaveController events handling

        private static void Controller_ControllerStatusChanged (object sender, ControllerStatusEventArgs args)
        {
            Console.WriteLine("ControllerStatusChange {0}", args.Status);
            ToggleDebug(true);
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
                ShowMenu();
                break;
            case ControllerStatus.Initializing:
                break;
            case ControllerStatus.Ready:
                // Query all nodes (Supported Classes, Routing Info, Node Information Frame, Manufacturer Specific)
//                controller.Discovery();
                ShowMenu();
                break;
            case ControllerStatus.Error:
                Console.WriteLine("\nEnter [+] to try reconnect\n");
                ShowMenu();
                break;
            }
            ToggleDebug(false);
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
