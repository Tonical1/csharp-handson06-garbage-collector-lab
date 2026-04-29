namespace GCLab;

class LeakySubscriber
{
    private static readonly List<LeakySubscriber> _registry = new();
    private Publisher _publisher;

    public LeakySubscriber(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.OnSomething += Handle;
        _registry.Add(this);
    }

    private void Handle() { /* noop */ }
}