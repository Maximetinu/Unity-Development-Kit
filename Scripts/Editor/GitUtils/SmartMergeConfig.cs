using UnityEditor;
using System.Threading;
using System.Diagnostics;

namespace UDK.Editor.GitUtils
{
    public static class SmartMergeConfig
    {
        static string MergetoolPath
        {
            get
            {
                string executable = EditorApplication.applicationPath;
                string editor = executable.Substring(0, executable.Length - "Unity.exe".Length);
                string mergetool = "Data/Tools/UnityYAMLMerge.exe";
                string m_mergetoolPath = editor + mergetool;
                m_mergetoolPath = m_mergetoolPath.Replace("/", "\\");
                return m_mergetoolPath;            
            }
        }

        static string ProjectPath
        {
            get => System.IO.Directory.GetCurrentDirectory();
        }

        [InitializeOnLoadMethod]
        static void Init()
        {
            UnityWindowFocus.OnUnityWindowFocusChanged += (focus) => {ConfigNow();};
        }

        public static void ConfigNow()
        {
            ProcessStartInfo procInfo = new ProcessStartInfo("cmd.exe");
            procInfo.WorkingDirectory = ProjectPath;
            procInfo.RedirectStandardOutput = true;
            procInfo.UseShellExecute = false;
            procInfo.CreateNoWindow = true;

            new Thread( () => RunCommands(procInfo)).Start();
        }

        static void RunCommands(ProcessStartInfo procInfo)
        {
            string command;

            command = "git config merge.tool unityyamlmerge";
            Exec(command, procInfo);

            Thread.Sleep(10);

            command = $"git config mergetool.unityyamlmerge.driver \"'{MergetoolPath}' merge -h -p --force %O %B %A %A\"";
            Exec(command, procInfo);
            
            Thread.Sleep(10);

            command = $"git config mergetool.unityyamlmerge.recursive binary";
            Exec(command, procInfo);

            Thread.Sleep(10);

            command = $"git config mergetool.unityyamlmerge.trustExitCode false";
            Exec(command, procInfo);
        }

        static void Exec(string command, ProcessStartInfo procInfo)
        {
            procInfo.Arguments = "/c" + command;
            Process.Start(procInfo);
        }
    }
}