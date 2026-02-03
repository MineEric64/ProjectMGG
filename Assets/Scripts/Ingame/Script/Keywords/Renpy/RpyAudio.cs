using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class RpyAudio : IStatement
    {
        public int Line { get; set; } = 0;

        public RpyAudioStates State { get; set; } = RpyAudioStates.None;
        public string Channel { get; set; }
        public IExpression Path { get; set; }

        public bool Loop { get; set; } = true;

        public IExpression fadein = null;
        public IExpression fadeout = null;
        public int isloop = -1; //renpy internal feature
        public IExpression volume = null;
        public bool if_changed = false;

        public string fadeease = "Linear";
        public float loopstart = -1f;
        public float loopend = -1f;

        public void Interpret()
        {
            //Predefined Path
            RpyAudio audio = this;
            string path2 = audio.GetPathToString();

            switch (path2)
            {
                case "ROOMIE_ROOKIE":
                    {
                        audio = new RpyAudio();
                        audio.State = State;
                        audio.Channel = Channel;
                        audio.Path = new StringLiteral("audio/bg_roomie_rookie");
                        audio.loopstart = 30.63f;
                        audio.loopend = 112.34f;
                        audio.isloop = 1;
                        break;
                    }
                case "STARBERRY_MILK":
                    {
                        audio = new RpyAudio();
                        audio.State = State;
                        audio.Channel = Channel;
                        audio.Path = new StringLiteral("audio/bg_starberry_milk");
                        audio.loopstart = 40.85f;
                        audio.loopend = 142.97f;
                        audio.isloop = 1;
                        break;
                    }
            }
            path2 = audio.GetPathToString();

            //Loop set
            if (isloop == -1)
            {
                switch (Channel)
                {
                    case "music":
                        Loop = true;
                        break;

                    case "sound":
                        Loop = false;
                        break;
                }
            }
            else if (isloop == 0) Loop = false;

            float fadein2 = 0f;
            float fadeout2 = 0f;
            float volume2 = 1f;

            if (fadein != null)
            {
                var fadein3 = fadein?.Interpret() as float?;

                if (fadein3 != null) fadein2 = fadein3.Value;
                else ExceptionManager.Throw("Can't cast fadein's value to float.", "Interpreter/RpyAudio", Line);
            }
            if (fadeout != null)
            {
                var fadeout3 = fadeout?.Interpret() as float?;

                if (fadeout3 != null) fadeout2 = fadeout3.Value;
                else ExceptionManager.Throw("Can't cast fadeout's value to float.", "Interpreter/RpyAudio", Line);
            }
            if (volume != null)
            {
                var volume3 = volume?.Interpret() as float?;

                if (volume3 != null) volume2 = volume3.Value;
                else ExceptionManager.Throw("Can't cast volume's value to float.", "Interpreter/RpyAudio", Line);
            }

            //Statement
            switch (State)
                {
                    case RpyAudioStates.Play:
                        IngameManagerV2.Instance.LetsPlay(audio, path2, fadein2, fadeout2, volume2);
                        break;

                    case RpyAudioStates.Stop:
                        fadeease = "OutCubic"; //temporary on fadeout, deprecated (TODO: implement ease custom syntax in script)
                        IngameManagerV2.Instance.LetsStop(audio, fadeout2);
                        break;

                    case RpyAudioStates.Queue:
                        break;

                    default:
                        break;
                }
        }

        public string GetFileName()
        {
            return GetPathToString();
        }

        public string GetPathToString()
        {
            if (Path != null)
            {
                if (Path is GetVariable t) return t.Name;
                else if (Path is StringLiteral s) return s.Value;
            }
            return string.Empty;
        }
    }

    public enum RpyAudioStates
    {
        None = 0,
        Play = 1,
        Stop = 2,
        Queue = 3
    }
}