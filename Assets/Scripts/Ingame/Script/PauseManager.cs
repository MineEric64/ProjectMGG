using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

using ProjectMGG.Ingame.Script.Keywords.Renpy;
using System;

namespace ProjectMGG.Ingame.Script
{
    public class PauseManager
    {
        public static bool Paused => Count > 0;
        public static int Count => _pauses.Count;
        public static event EventHandler OnCompleted;

        private static List<Pause> _pauses = new List<Pause>();
        private static List<Pause> _removed = new List<Pause>();

        public static void Add(Pause pause)
        {
            _pauses.Add(pause);
        }

        public static bool Remove(bool includeHard = false)
        {
            bool remove = false;

            for (int i = 0; i < _pauses.Count; i++)
            {
                Pause pause = _pauses[i];

                if (includeHard || !pause.Hard)
                {
                    _removed.Add(pause);
                    remove = true;
                    break;
                }
            }

            return remove;
        }

        public static IEnumerator Loop()
        {
            int countPrev = 0;

            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                for (int i = 0; i < _pauses.Count; i++)
                {
                    Pause pause = _pauses[i];
                    
                    pause.CurrentDelay += 0.01f;
                    if (pause.CurrentDelay >= pause.Delay) _removed.Add(pause);
                }

                if (countPrev > 0 && Count - _removed.Count == 0) OnCompleted.Invoke(null, null);

                for (int i = 0; i < _removed.Count; i++)
                {
                    Pause pause = _removed[i];

                    pause.ActionAfter?.Invoke();
                    _pauses.Remove(pause);
                }
                _removed.Clear();

                countPrev = Count;
            }
        }

        public static void Clear()
        {
            _pauses.Clear();
            _removed.Clear();
        }
    }
}
