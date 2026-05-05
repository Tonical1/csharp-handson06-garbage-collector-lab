namespace GCLab;
using System.Diagnostics;

class Program
{
    // IMPORTANTE: ESTE CÓDIGO CONTÉM PROBLEMAS PROPOSITAIS.
    // Os alunos devem implementar as correções para chegar ao final com "GC limpo".
    static void Main()
    {
        Console.WriteLine("=== GCLab - Versão com Problemas ===");
        Console.WriteLine($"GC Server Mode: {System.Runtime.GCSettings.IsServerGC}\n");

        var tracker = new IssueTracker();

        // 1) Vazamento por evento não desinscrito
        var publisher = new Publisher();
        var subscriber = new LeakySubscriber(publisher);
        tracker.Track("subscriber", subscriber);

        // 2) LOH + cache estático sem política de expiração
        var lohBuffer = BigBufferHolder.Run();
        tracker.Track("lohBuffer", lohBuffer);

        // 3) Pinned buffer mantido por muito tempo
        var pinner = new Pinner();
        var pinned = pinner.PinLongTime();
        tracker.Track("pinnedBuffer", pinned);

        // 4) Concatenação de string ineficiente
        Console.WriteLine("--- Comparação entre métodos de concatenação ---");

        var sw1 = Stopwatch.StartNew();
        var payload = ConcatWork.Bad();
        sw1.Stop();

        Console.WriteLine($"Old Payload length: {payload.Length} | Time: {sw1.ElapsedMilliseconds} ms");

        var sw2 = Stopwatch.StartNew();
        var payloadnew = ConcatWork.Good();
        sw2.Stop();

        Console.WriteLine($"New payload length: {payloadnew.Length} | Time: {sw2.ElapsedMilliseconds} ms");

        // 5) Recurso externo sem Dispose (usar finalizer como 'rede de segurança')
        var logger = new Logger("log.txt");
        logger.WriteLines(10);
        tracker.Track("logger", logger);

        // Dispara evento para "usar" o subscriber
        publisher.Raise();

        // Remover referências locais (mas problemas permanecem)
        subscriber = null;
        publisher = null;
        pinned = null;
        logger = null;
        lohBuffer = null;

        // Força coletas e verifica sobreviventes
        GCHelpers.FullCollect();
        tracker.Report();

        Console.WriteLine(tracker.HasSurvivors
            ? "\n❌ Existem sobreviventes indesejados. Sua missão: corrigir o código e rodar novamente."
            : "\n✅ GC limpo: nenhuma referência indesejada permaneceu viva.");
    }
}