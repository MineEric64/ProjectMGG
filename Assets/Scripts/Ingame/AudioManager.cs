using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ProjectMGG.Ingame.Script.Keywords.Renpy;

namespace ProjectMGG.Ingame
{
    public class AudioManager
    {
        private static Queue<RpyAudio> _q = new Queue<RpyAudio>();

        public static void AddToQueue(RpyAudio audio)
        {
            _q.Enqueue(audio);
        }

        //reference: https://youtu.be/CThc-Nnc91Q
        /// <summary>
        /// Management for Queue & Loop system
        /// </summary>
        public static IEnumerator Loop(AudioSource player)
        {
            while (true)
            {
                yield return null;

                //Queue

                //Loop system
                if (player.isPlaying)
                {
                    var currentMusic = IngameManagerV2.Instance.CurrentMusic;

                    bool ls_enabled = !(currentMusic.loopstart < 0f);
                    bool le_enabled = !(currentMusic.loopend < 0f);
                    float ls = ls_enabled ? currentMusic.loopstart : 0f;
                    float le = le_enabled ? currentMusic.loopend : player.clip.length;

                    if ((ls_enabled || le_enabled) && (player.time >= le))
                    {
                        player.time = ls;
                    }
                }
            }
        }

        public static void Clear()
        {
            _q.Clear();
        }
    }
}