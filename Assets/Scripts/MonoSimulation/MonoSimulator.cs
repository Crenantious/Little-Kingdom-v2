using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public class MonoSimulator : MonoBehaviour
    {
        private readonly List<UpdatableInfo> updatables = new();
        private readonly Dictionary<IUpdatable, int> updateableInfoIndex = new();

        public void RegisterForUpdate(IUpdatable updatable, float delayInSeconds = 0)
        {
            UpdatableInfo info = new(updatable, delayInSeconds);
            updatables.Add(info);
            updateableInfoIndex.Add(updatable, updatables.Count - 1);
        }

        public void UnregisterForUpdate(IUpdatable updatable)
        {
            updatables.RemoveAt(updateableInfoIndex[updatable]);
            updateableInfoIndex.Remove(updatable);
        }

        private void Update()
        {
            foreach (UpdatableInfo info in updatables)
            {
                info.Cooldown -= Time.deltaTime;
                if (info.Cooldown <= 0)
                {
                    info.Cooldown = info.Delay;
                    info.Updatable.Update();
                }
            }
        }
    }
}