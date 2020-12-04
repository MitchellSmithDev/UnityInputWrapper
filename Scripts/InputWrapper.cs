public abstract class InputWrapper
{
    EventHelper updateHelper = null;
    public bool HelperExists => updateHelper != null;

    int[] input = new int[4];
    public int this[int i] => input[i];

    public InputWrapper(EventHelper eventHelper)
    {
        if(eventHelper != null)
        {
            updateHelper = eventHelper;
            updateHelper.Add(BaseUpdate);
        } else
        {
            throw new System.InvalidOperationException("Given update helper is null.");
        }
    }

    public void SetHelper(EventHelper eventHelper)
    {
        if(!HelperExists)
        {
            updateHelper = eventHelper;
            updateHelper.Add(BaseUpdate);
        } else
        {
            throw new System.InvalidOperationException("Cannot set a new update helper while the initial helper exists.");
        }
    }

    void BaseUpdate()
    {
        Update();

        input[1] = input[3] & ~input[0];
        input[2] = ~input[3] & input[0];
        input[0] = input[3];
        input[3] = 0;
    }

    protected abstract void Update();

    public void Add(int button) { input[3] |= (1 << button); }
    public void Remove(int button) { input[3] &= ~(1 << button); }
    public void Clean() { input[3] = 0; }

    public bool Get(int button, int i = 0) => input[i] == (input[i] | (1 << button));
    public bool Down(int button) => Get(button, 1);
    public bool Up(int button) => Get(button, 2);
}