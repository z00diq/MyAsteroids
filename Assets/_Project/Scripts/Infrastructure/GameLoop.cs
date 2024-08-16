using Assets.Models;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class GameLoop
    {
        private List<IStartable> _startable = new List<IStartable>();
        private List<IUpdatable> _updatable = new List<IUpdatable>();
        private List<IFixedUpdatable> _fixedUpdatable = new List<IFixedUpdatable>();

        public void Start()
        {
            for (int i = 0; i < _startable.Count; i++)
                _startable[i].Start();
            
        }

        public void Update()
        {
            for (int i = 0; i < _updatable.Count; i++)
                _updatable[i].Update();
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatable.Count; i++)
                _fixedUpdatable[i].FixedUpdate();
        }

        public void AddToStarable(IStartable startable)
        {
            _startable.Add(startable);
        }

        public void AddToUpdatable(IUpdatable updatable)
        {
            _updatable.Add(updatable);
        }

        public void RemoveFromUpdatable(IUpdatable updatable)
        {
            _updatable.Remove(updatable);
        }

        public void AddToFixedUpdatable(IFixedUpdatable fixedUpdatable)
        {
            _fixedUpdatable.Add(fixedUpdatable);
        }
    }
}
