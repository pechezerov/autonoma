namespace PrettyScreen.Core
{
    public enum DataQuality
    {
        Ok,
        Suspicious,
        Bad
    }

    public enum DataSource
    {
        Field,
        Control
    }

    public enum WorkState
    {
        Off,
        Pending,
        On,
        Error
    }
}