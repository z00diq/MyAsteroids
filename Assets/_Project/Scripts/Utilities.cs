using UnityEngine;

namespace Assets
{
    public class Utilities
    {
        public static Vector2 ScreenBounds;

        public Utilities(Vector2 screenBounds)
        {
            ScreenBounds = screenBounds;
        }

        public static bool IsPositionTooFar(Vector3 position, Vector2 size, float tooFarDistance=0f)
        {
            Vector3 absPosition = new Vector3();
            Vector3 absOutBounds = new Vector3();

            absOutBounds.x = ScreenBounds.x + tooFarDistance;
            absOutBounds.y = ScreenBounds.y + tooFarDistance;

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

            float negativeAsterodPositionY = Random.Range(-ScreenBounds.y - outBoundsDepth, -ScreenBounds.y);
            float positiveAsterodPositionY = Random.Range(ScreenBounds.y, ScreenBounds.y + outBoundsDepth);
            float negativeAsterodPositionX = Random.Range(-ScreenBounds.x - outBoundsDepth, -ScreenBounds.x);
            float positiveAsterodPositionX = Random.Range(ScreenBounds.x, -ScreenBounds.x + outBoundsDepth);

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
