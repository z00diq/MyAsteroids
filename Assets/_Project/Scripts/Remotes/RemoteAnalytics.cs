using Assets.Models;
using System.Threading.Tasks;

namespace Assets._Project.Scripts.Remotes
{
    public abstract class  RemoteAnalytics
    {
        protected const string START_GAME_EVENT_NAME = "Start Game";
        protected const string END_GAME_EVENT_NAME = "End Game";
        protected const string LASER_USE_EVENT_NAME = "Laser Used";


        private int _fireCount;
        private int _laserFireCount;
        private int _asteroidsDestroyedCount;
        private int _ufoDestroyedCount;

        protected int FireCount => _fireCount;
        protected int LaserFireCount => _laserFireCount;
        protected int AsteroidsDestroyedCount => _asteroidsDestroyedCount;
        protected int UfoDestroyedCount => _ufoDestroyedCount;

        public abstract Task InitializeAsync();

        public abstract void StartGameEvent();

        public abstract void LaserUseEvent();

        public virtual void EndGameEvent()
        {
            _fireCount = 0;
            _laserFireCount = 0;
            _asteroidsDestroyedCount = 0;
            _ufoDestroyedCount = 0;
        }

        public void IncreaseFireCount()
        {
            _fireCount++;
        }

        public void IncreaseLaserFireCount()
        {
            _laserFireCount++;
        }

        public void IncreaseAsteroidsDestroyedCount()
        {
            _asteroidsDestroyedCount++;
        }

        public void IncreaseUfoDestroyedCount()
        {
            _ufoDestroyedCount++;
        }
    }
}
