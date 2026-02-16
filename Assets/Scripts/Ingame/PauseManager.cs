using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

using UnityEngine;

using ProjectMGG.Ingame.Script.Keywords.Renpy;

namespace ProjectMGG.Ingame
{
    public class PauseManager
    {
        public static bool Paused => Count > 0;
        public static int Count => _pauses.Count;
        public static event EventHandler OnCompleted;

        private static List<Pause> _pauses = new List<Pause>();
        private static SortedSet<Pause> _removed = new SortedSet<Pause>();

        public static void Add(Pause pause)
        {
            _pauses.Add(pause);
        }

        //TODO: change to Priority Queue (this algorithm has expensive cost: O(2N))
        public static bool Remove(bool includeHard = false)
        {
            bool remove = false;

            if (includeHard) //Hard first
            {
                for (int i = 0; i < _pauses.Count; i++)
                {
                    Pause pause = _pauses[i];

                    if (pause.Hard && !_removed.Contains(pause))
                    {
                        _removed.Add(pause);
                        remove = true;
                        break;
                    }
                }
            }

            if (!remove)
            {
                for (int i = 0; i < _pauses.Count; i++)
                {
                    Pause pause = _pauses[i];

                    if (!pause.Hard && !_removed.Contains(pause))
                    {
                        _removed.Add(pause);
                        remove = true;
                        break;
                    }
                }
            }

            return remove;
        }

        /// <summary>
        /// Not recommended, because of thread synchronization issue (So don't use it when PauseManager is looping)
        /// </summary>
        /// <param name="includeHard"></param>
        /// <returns></returns>
        public static bool RemoveImmediate(bool includeHard = false)
        {
            bool removed = Remove(includeHard);
            RemovePausesInternal();

            return removed;
        }

        public static IEnumerator Loop()
        {
            int countPrev = 0;

            while (true)
            {
                yield return null;

                //conditions to check for loop
                if (IngameManagerV2.Instance != null && !IngameManagerV2.Instance.Focused) continue; //Pause menu, not Ingame

                for (int i = 0; i < _pauses.Count; i++)
                {
                    Pause pause = _pauses[i];
                    
                    pause.CurrentDelay += Time.deltaTime;
                    if (pause.CurrentDelay >= pause.Delay) _removed.Add(pause);
                }

                if (countPrev > 0 && Count - _removed.Count == 0) OnCompleted?.Invoke(null, null);

                if (_removed.Count > 0) RemovePausesInternal();
                countPrev = Count;
            }
        }

        private static void RemovePausesInternal()
        {
            foreach (var pause in _removed)
            {
                pause.ActionAfter?.Invoke();
                _pauses.Remove(pause);
            }
            _removed.Clear();
        }

        public static void Clear()
        {
            _pauses.Clear();
            _removed.Clear();
        }
    }
}
