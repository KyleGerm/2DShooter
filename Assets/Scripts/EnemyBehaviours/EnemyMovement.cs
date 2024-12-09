using UnityEngine;

public class EnemyMovement 
{
    private EnemyController controller;
    protected Vector3 NextDestination { get; set; }
    public float speed = 3;
    float range;
    bool tooClose;
    public EnemyMovement(EnemyController controller)
    {
        NextDestination = Vector3.zero;
        this.controller = controller;
        FindNextDestination();
    }

    /// <summary>
    /// Sets the destination of the enemy to a new location within range of the player
    /// </summary>
    private void FindNextDestination()
    {
        range = controller.Weapon.Range;
        Vector3 Target = controller.Player.position;
        Vector2 randomDir = Random.insideUnitCircle * range;
        Vector3 dir = new Vector3(randomDir.x, randomDir.y, 0);
        NextDestination = Target + dir;

    }

    /// <summary>
    /// Allows the player to make decisions on how to move based on conditions
    /// </summary>
    public void Move()
    {
        if (controller.Distance > range)
        {
            tooClose = false;
        }


        if (TargetPositionNotInRange() || AtDestination())
        {
            FindNextDestination();
            return;
        }
        else if (InRangeAndNotTooClose()) 
        {  
            CheckIfStillNotTooClose();
            return;
        }
        else
        {
            // if we make it here, we are not in a desirable location, and should continue moving to the target location
           MoveToNextDestination();
            return;
        }
    }

    private void MoveToNextDestination()
    {
        controller.gameObject.transform.position =
               Vector3.MoveTowards(controller.gameObject.transform.position, NextDestination, Time.deltaTime * speed);
    }

    private bool TargetPositionNotInRange()
    {
        return Vector2.Distance(controller.Player.transform.position, NextDestination) > range;
    }

    private bool InRangeAndNotTooClose()
    {
        return controller.Distance < range && !tooClose;
    }

    private void CheckIfStillNotTooClose()
    {
        if (controller.Distance < range / 2) //if we are too close, find a new position
        {  
            return; 
        }
            tooClose = true;
            FindNextDestination();
        
    }

    private bool AtDestination()
    {
        return controller.gameObject.transform.position == NextDestination;
    }

}
