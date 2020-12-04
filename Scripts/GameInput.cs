using UnityEngine;

public enum ButtonCode : int
{
    None,
    Up,
    Left,
    Down,
    Right
}

public class GameInput : InputWrapper
{
    public Vector2 Direction { get; private set; }

    public GameInput(EventHelper eventHelper) : base(eventHelper) { }

    protected override void Update()
    {
        SOCDClean();

        Vector2 newDirection = Vector2.zero;
        
        if(Get(ButtonCode.Left, 3))
        {
            newDirection.x--;
        } else if(Get(ButtonCode.Right, 3))
        {
            newDirection.x++;
        }
        
        if(Get(ButtonCode.Up, 3))
        {
            newDirection.y++;
        } else if(Get(ButtonCode.Down, 3))
        {
            newDirection.y--;
        }

        Direction = newDirection;
    }

    // Simultaneous Opposing Cardinal Direction Cleaning, favors the last direction given
    void SOCDClean()
    {
        if(Get(ButtonCode.Left, 3) && Get(ButtonCode.Right, 3))
        {
            if(!Get(ButtonCode.Left))
                Remove(ButtonCode.Left);
            if(!Get(ButtonCode.Right))
                Remove(ButtonCode.Right);
        }

        if(Get(ButtonCode.Up, 3) && Get(ButtonCode.Down, 3))
        {
            if(!Get(ButtonCode.Up))
                Remove(ButtonCode.Up);
            if(!Get(ButtonCode.Down))
                Remove(ButtonCode.Down);
        }
    }

    public void Add(ButtonCode button) { Add((int)button); }
    public void Remove(ButtonCode button) { Remove((int)button); }

    // Snaps given direction to 45 degree angle, then sets cardinal directions accordingly
    public void Add(Vector2 direction)
    {
        if(direction == Vector2.zero)
            return;
        
        Vector2 snapDirection = direction.normalized;
        float angle = Vector3.Angle(snapDirection, Vector3.up);

        if(angle < 22.5f)
        {
            Add(ButtonCode.Up);
        } else if(angle > 157.5f)
        {
            Add(ButtonCode.Down);
        } else
        {
            float deltaAngle = (Mathf.Round(angle / 45f) * 45f) - angle;

            Vector3 axis = Vector3.Cross(Vector3.up, snapDirection);
            Quaternion rotation = Quaternion.AngleAxis(deltaAngle, axis);

            snapDirection = rotation * snapDirection;

            if(snapDirection.x < 0)
                Add(ButtonCode.Left);
            else if(snapDirection.x > 0)
                Add(ButtonCode.Right);

            if(snapDirection.y < 0)
                Add(ButtonCode.Down);
            else if(snapDirection.y > 0)
                Add(ButtonCode.Up);
        }
    }

    public bool Get(ButtonCode button, int i = 0) => Get((int)button, i);
    public bool Down(ButtonCode button) => Down((int)button);
    public bool Up(ButtonCode button) => Up((int)button);
}