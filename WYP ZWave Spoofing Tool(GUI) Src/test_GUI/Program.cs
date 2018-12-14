using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ZWaveLib;
using ZWaveLib.CommandClasses;
using System.Text.RegularExpressions;

namespace WYP_ZWave_Spoofing_Tool
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MdiMain());
        }
    }
}
