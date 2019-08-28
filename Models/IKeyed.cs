namespace ConsoleTemplate.Models
{
    public interface IKeyed<TKey>
    {
        TKey Key { get; }
    }
}
