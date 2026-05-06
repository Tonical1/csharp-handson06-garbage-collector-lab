namespace GCLab;

class IssueTracker
{
    private readonly List<(string label, WeakReference wr)> _list = new();
    public bool HasSurvivors { get; private set; }

    public void Track(string label, object obj)
    {
        if (obj == null) return;
        _list.Add((label, new WeakReference(obj)));
    }

    public void Report()
    {
        Console.WriteLine("\n--- Verificação de sobreviventes (WeakReference) ---");
        int alive = 0;
        foreach (var (label, wr) in _list)
        {
            var isAlive = wr.IsAlive;
            if (isAlive) alive++;
            Console.WriteLine($"{label}: {(isAlive ? "vivo" : "coletado")}");
        }
        HasSurvivors = alive > 0;
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine($"Gen0: {GC.CollectionCount(0)} | Gen1: {GC.CollectionCount(1)} | Gen2: {GC.CollectionCount(2)}");
    }
}