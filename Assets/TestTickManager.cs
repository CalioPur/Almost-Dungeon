using UnityEngine;

public class TestTickManager : MonoBehaviour
{
    private int entityId;
    
    void Start()
    {
        // Subscribe to movement events for an entity with a unique ID
        entityId = GetHashCode();
        TickManager.SubscribeToMovementEvent(MovementType.Hero, HandleHeroMovement, entityId);
        TickManager.SubscribeToMovementEvent(MovementType.Monster, HandleMonsterMovement, entityId);
        TickManager.SubscribeToMovementEvent(MovementType.Trap, HandleTrapMovement, entityId);
        
    }
    
    void HandleTrapMovement()
    {
        // Handle trap movement logic here
        Debug.Log($"Trap with ID {entityId} is moving!");
    }
    
    void HandleMonsterMovement()
    {
        // Handle monster movement logic here
        Debug.Log($"Monster with ID {entityId} is moving!");
    }
    
    void HandleHeroMovement()
    {
        // Handle hero movement logic here
        Debug.Log($"Hero with ID {entityId} is moving!");
    }
    
    void Update()
    {
        // Example: Unsubscribe from movement events after a certain condition is met
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TickManager.UnsubscribeFromMovementEvent(MovementType.Monster, entityId);
            Debug.Log($"Monster with ID {entityId} unsubscribed from movement events.");
        }
    }
}