namespace SimpleDota2Editor
{
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void UnExecute();
    }
}