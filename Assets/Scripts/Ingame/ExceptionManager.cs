using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectMGG.UI;

namespace ProjectMGG.Ingame
{
    public class ExceptionManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Throws the error and show log ([scriptName] : [message])
        /// </summary>
        public static void Throw(string message, string scriptName = "", int lineCount = -1)
        {
            string message2 = $"{scriptName} : {message}";
            //string message3 = $"Error occured!\n\nThe message:\n{message}";

            if (lineCount > 0)
            {
                message2 += $" : (Line {lineCount})";
                //message3 += $"\n(Line {lineCount})";
            }

            Debug.LogError(message2);
            PauseManager.Add(Script.Keywords.Renpy.Pause.GetInfinity(true));
            MessageBox.Instance?.Error(message2, onSubmit: (_) => { PauseManager.Remove(true); });

            //if (UnityEditor.EditorUtility.DisplayDialog(scriptName, message3, "OK"))
            //{
            //    return;
            //}
        }
    }
}