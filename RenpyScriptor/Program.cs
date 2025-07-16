using System.IO;
using System.Text;
using TextCopy;

namespace RenpyScriptor
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tools for Project MGG - RenpyScriptor";

            while (true)
            {

                Console.Clear();
                Console.WriteLine("1. Script Convert from Text File");
                Console.WriteLine("2. Script Convert from Clipboard");
                Console.WriteLine("3. Exit");
                Console.Write("Input : ");
                string? option = Console.ReadLine();

                if (option == "1")
                {
                    Console.Write("Text File Path : ");
                    string? path = Console.ReadLine();
                    if (ConvertToScriptFromFile(path)) Console.WriteLine("Converted successfully.");
                }
                else if (option == "2")
                {
                    string? content = ClipboardService.GetText();

                    if (!string.IsNullOrEmpty(content))
                    {
                        string script = ConvertToScript(content);
                        ClipboardService.SetText(script);
                        Console.WriteLine("Converted successfully. The content is copied to Clipboard.");
                    }
                    else
                    {
                        Console.WriteLine("The content is empty. Try again.");
                    }
                }
                else
                {
                    return;
                }

                Console.ReadLine();
            }
        }

        static string ConvertToScript(string content)
        {
            /*
             * [input sample]
             * 세아: “저… 그리고······.”
                세아: “저.. 저 방송부 선배인 한세아라고 하니깐 앞으로 잘 부탁해요!!!”
                선배의 쩌렁쩌렁한 목소리가 교실에 울려펴졌다.
                (Nemo Neko 브금이 멈추며 리버브가 울려퍼진다)
             */

            /*
             * [output sample]
             * seah “저… 그리고······.”
             * seah “저.. 저 방송부 선배인 한세아라고 하니깐 앞으로 잘 부탁해요!!!”
             * "선배의 쩌렁쩌렁한 목소리가 교실에 울려펴졌다."
             * # Nemo Neko 브금이 멈추며 리버브가 울려퍼진다
             */

            string[] texts = SplitbyLine(content);
            Dictionary<string, string> map = new Dictionary<string, string>();
            StringBuilder sbDefine = new StringBuilder();
            StringBuilder sbStart = new StringBuilder();
            StringBuilder sbFinal = new StringBuilder();

            bool previousText = false;

            foreach (string text in texts)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    previousText = false;
                    continue;
                }

                var script = ScriptSyntax.Interpret(text);

                if (script.Method == Methods.Dialog)
                {
                    if (!map.ContainsKey(script.Name))
                    {
                        Console.Write($"Not found the name '{script.Name}'. Please choose the variable name : ");
                        string? nameVar = Console.ReadLine() ?? string.Empty;
                        script.NameVar = nameVar;
                        map[script.Name] = nameVar;
                    }
                    else if (string.IsNullOrWhiteSpace(script.NameVar))
                    {
                        script.NameVar = map[script.Name];
                    }
                }

                if (!previousText)
                {
                    sbStart.AppendLine(script.ToRenpy());
                }
                else
                {
                    sbStart.Remove(sbStart.Length - 3, 3);
                    sbStart.Append("{p}");
                    sbStart.AppendLine(script.ToRenpy(false).TrimStart('\"'));
                }
                previousText = true;
            }

            foreach (string key in map.Keys)
            {
                sbDefine.Append("define ");
                sbDefine.Append(map[key]);
                sbDefine.Append(" = Character(\"");
                sbDefine.Append(key);
                sbDefine.AppendLine("\")");
            }
            sbDefine.AppendLine();

            sbFinal.Append(sbDefine.ToString());
            sbFinal.AppendLine("label start:");
            sbFinal.Append(sbStart);

            return sbFinal.ToString();
        }

        static bool ConvertToScriptFromFile(string? path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Text File doesn't exists.");
                return false;
            }

            string text = File.ReadAllText(path);
            string content = ConvertToScript(text);

            string parentFullPath = Directory.GetParent(path).FullName;
            string fileName = Path.GetFileNameWithoutExtension(path);
            File.WriteAllText(Path.Combine(parentFullPath, $"{fileName}.rpy"), content);

            return true;
        }

        //This function code is from https://github.com/MineEric64/UniConverter-Project/blob/master/MainProject.vb#L1046
        static string[] SplitbyLine(string text)
        {
            const char CR = '\r';
            const char LF = '\n';

            if (text.Contains(CR) && !text.Contains(LF)) return text.Split(CR); //Mac
            else if (!text.Contains(CR) && text.Contains(LF)) return text.Split(LF); //Linux

            return text.Split("\r\n"); //Windows (CRLF)
        }
    }
}

