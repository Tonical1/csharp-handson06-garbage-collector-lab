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
        InitProgram( tracker );

        Console.WriteLine(tracker.HasSurvivors
            ? "\n❌ Existem sobreviventes indesejados. Sua missão: corrigir o código e rodar novamente."
            : "\n✅ GC limpo: nenhuma referência indesejada permaneceu viva.");
    }

    static void InitProgram(IssueTracker tracker)
    {

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


        // Força coletas e verifica sobreviventes
        GCHelpers.FullCollect();
        tracker.Report();
    }
}