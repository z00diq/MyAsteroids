using Assets.Models;

public interface IEnnemyInteractable
{
    public void Impact(Destroyable enemy)
    {
        enemy.Destroy();
    }
}