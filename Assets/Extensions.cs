using Assets.Scripts;
using UnityEngine;

namespace Assets
{
    public static class Extensions
    {
        public static bool IsPositionTooFar(Vector3 position, View view, float tooFarDistance=10f)
        {
            if (position.x + view.ModelSize.x  > Game.Instance.ScreenBounds.x + tooFarDistance ||
                position.x - view.ModelSize.x  < -Game.Instance.ScreenBounds.x - tooFarDistance)
                return true;

            if (position.y + view.ModelSize.y  > Game.Instance.ScreenBounds.y + tooFarDistance ||
                position.y < -view.ModelSize.y  - Game.Instance.ScreenBounds.y - tooFarDistance)
                return true;

            return false;
        }

        public static Vector3 CalculatePositionOutsideBounds(float outBoundsDepth, View view)
        {
            Vector3 newPosition = Vector3.one;
        
            float negativeAsterodPositionY = Random.Range(-Game.Instance.ScreenBounds.y - outBoundsDepth, -Game.Instance.ScreenBounds.y);
            float positiveAsterodPositionY = Random.Range(Game.Instance.ScreenBounds.y , Game.Instance.ScreenBounds.y + outBoundsDepth);
            float negativeAsterodPositionX = Random.Range(-Game.Instance.ScreenBounds.x - outBoundsDepth, -Game.Instance.ScreenBounds.x);
            float positiveAsterodPositionX = Random.Range(Game.Instance.ScreenBounds.x, -Game.Instance.ScreenBounds.x + outBoundsDepth);

            float randomDirection = Random.Range(1f, 100f);

            if(randomDirection<25f)
                newPosition = new Vector3(negativeAsterodPositionX, negativeAsterodPositionY);
            else if (randomDirection<50f)
                newPosition = new Vector3(positiveAsterodPositionX, positiveAsterodPositionY);
            else if (randomDirection<75f)
                newPosition = new Vector3(negativeAsterodPositionX, positiveAsterodPositionY);
            else
                newPosition = new Vector3(positiveAsterodPositionX, negativeAsterodPositionY);

            return newPosition;
        }
    }

    
}
