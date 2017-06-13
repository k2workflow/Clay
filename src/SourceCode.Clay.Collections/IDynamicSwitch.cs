namespace SourceCode.Clay.Collections.Generic
{
    public interface IDynamicSwitch<in K, T>
    {
        T this[K key] { get; }

        int Count { get; }

        bool ContainsKey(K key);

        bool TryGetValue(K key, out T value);
    }
}
