namespace GCLab;
using System.Diagnostics;
using System.Runtime.CompilerServices;

class Program
{
    // IMPORTANTE: ESTE CÓDIGO CONTÉM PROBLEMAS PROPOSITAIS.
    // Os alunos devem implementar as correções para chegar ao final com "GC limpo".
    static void Main()
    {
        Console.WriteLine("=== GCLab - Versão com Problemas ===");
        Console.WriteLine($"GC Server Mode: {System.Runtime.GCSettings.IsServerGC}\n");

        var tracker = new IssueTracker();
        InitProgram( tracker );

        Console.WriteLine(tracker.HasSurvivors
            ? "\n❌ Existem sobreviventes indesejados. Sua missão: corrigir o código e rodar novamente."
            : "\n✅ GC limpo: nenhuma referência indesejada permaneceu viva.");
    }

    static void InitProgram(IssueTracker tracker)
    {
        // 1) Vazamento por evento não desinscrito
        var publisher = new Publisher();
        var subscriber = new LeakySubscriber(publisher);
        tracker.Track("subscriber", subscriber);
        publisher.Raise();

        subscriber.Dispose();
        LeakySubscriber.Clear();
        publisher = null;

        // 2) LOH + cache estático sem política de expiração
        var lohBuffer = BigBufferHolder.Run();
        tracker.Track("lohBuffer", lohBuffer);

        BigBufferHolder.Clear();
        lohBuffer = null;

        // 3) Pinned buffer mantido por muito tempo
        var pinner = new Pinner();
        var pinned = pinner.PinLongTime();
        tracker.Track("pinnedBuffer", pinned);

        pinner.Dispose();
        pinner = null;
        pinned = null;

        // 4) Concatenação de string ineficiente
        Console.WriteLine("--- Comparação entre métodos de concatenação ---");

        var sw2 = Stopwatch.StartNew();
        var payloadnew = ConcatWork.Good();
        sw2.Stop();
        Console.WriteLine($"New payload length: {payloadnew.Length} | Time: {sw2.ElapsedMilliseconds} ms");

        // 5) Recurso externo sem Dispose (usar finalizer como 'rede de segurança')
        using var logger = new Logger("log.txt");
        logger.WriteLines(10);
        tracker.Track("logger", logger);

        logger.Dispose();

        // Força coletas e verifica sobreviventes
        GCHelpers.FullCollect();
        tracker.Report();
    }
}