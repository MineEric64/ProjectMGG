using System.Text;

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
                Console.WriteLine("1. Script Convert");
                Console.WriteLine("2. Exit");
                Console.Write("Input : ");
                string? option = Console.ReadLine();

                if (option == "1")
                {
                    Console.Write("Text File Path : ");
                    string? path = Console.ReadLine();
                    if (ConvertToScript(path)) Console.WriteLine("Converted successfully.");
                }
                else
                {
                    return;
                }

                Console.ReadLine();
            }
        }

        static bool ConvertToScript(string? path)
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

            if (!File.Exists(path))
            {
                Console.WriteLine("Text File doesn't exists.");
                return false;
            }

            string[] texts = File.ReadAllLines(path);
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

            string parentFullPath = Directory.GetParent(path).FullName;
            string fileName = Path.GetFileNameWithoutExtension(path);
            File.WriteAllText(Path.Combine(parentFullPath, $"{fileName}.rpy"), sbFinal.ToString());

            return true;
        }
    }
}

