using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets
{
    public static class Utilities
    {
        public static bool IsPositionTooFar(Vector3 position, Vector2 size, float tooFarDistance=0f)
        {
            Vector3 absPosition = new Vector3();
            Vector3 absOutBounds = new Vector3();

            absOutBounds.x = Game.Instance.ScreenBounds.x + tooFarDistance;
            absOutBounds.y = Game.Instance.ScreenBounds.y + tooFarDistance;

            absPosition.x = Mathf.Abs(position.x) + size.x;
            absPosition.y = Mathf.Abs(position.y) + size.y;

            if (absPosition.x > absOutBounds.x
                || absPosition.y > absOutBounds.y)
                return true;

            return false;
        }

        public static Vector3 CalculatePositionOutsideBounds(float outBoundsDepth)
        {
            Vector3 newPosition;
        
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
