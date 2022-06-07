namespace Autonoma.UI.Configuration.Abstractions
{
    public interface IProjectSerializer
    {
        string SerializeProject(IProject value);

        IProject DeserializeProject<T>(string path) where T: IProject;
    }
}
