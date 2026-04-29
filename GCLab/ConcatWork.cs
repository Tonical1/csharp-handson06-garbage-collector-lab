namespace GCLab;

// ===================================
// 4) Concatenação de string ineficiente
// ===================================
static class ConcatWork
{
    public static string Bad()
    {
        string s = string.Empty;
        for (int i = 0; i < 50_000; i++)
            s += i;
        return s;
    }    
}
