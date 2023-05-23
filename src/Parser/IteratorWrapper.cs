namespace LFAF_LABORATORY;

public class IteratorWrapper<T>
{
    private IEnumerator<T> m_iterator;

    public IteratorWrapper(IEnumerator<T> iterator)
    {
        m_iterator = iterator;
    }

    public bool MoveNext()
    {
        return m_iterator.MoveNext();
    }

    public T Current
    {
        get { return m_iterator.Current; }
    }
}
