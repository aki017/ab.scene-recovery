using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;

[InitializeOnLoad]
public static class AbSceneRecoveryHook
{
    static AbSceneRecoveryHook()
    {
        EditorApplication.playModeStateChanged += PlayModeStateHook;
    }

    private static void PlayModeStateHook(PlayModeStateChange state)
    {
        if (!Directory.Exists("Temp/__Backupscenes"))
        {
            return;
        }

        foreach (var file in Directory.GetFiles("Temp/__Backupscenes"))
        {
            var md5 = CalcMd5(File.ReadAllBytes(file));
            Directory.CreateDirectory("Library/SceneBackup");
            var backupFileName = $"Library/SceneBackup/{md5}.backup";
            if (File.Exists(backupFileName))
            {
                File.SetLastWriteTime(backupFileName, DateTime.Now);
            }
            else
            {
                File.Copy(file, backupFileName);
            }
        }
    }

    private static string CalcMd5(byte[] bytes)
    {
        using (var md5 = MD5.Create())
        {
            var raw = md5.ComputeHash(bytes);
            md5.Clear();

            var sb = new StringBuilder();
            foreach (byte b in raw)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
