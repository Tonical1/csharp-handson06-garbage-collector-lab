namespace GCLab;

class LeakySubscriber : IDisposable
{
    private static readonly List<LeakySubscriber> _registry = new();
    private Publisher _publisher;

    public LeakySubscriber(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.OnSomething += Handle;
        _registry.Add(this);
    }

    public void Dispose()
    {
        if (_publisher != null)
        {
            _publisher.OnSomething -= Handle;
            _publisher = null;
        }
        _registry.Remove(this);
    }

    private void Handle() { /* noop */ }
}