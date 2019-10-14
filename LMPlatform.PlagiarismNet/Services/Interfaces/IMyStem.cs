using System.Collections.Generic;

namespace LMPlatform.PlagiarismNet.Services.Interfaces
{
    public interface IMyStem
    {
        List<string> Parse(string paramString);
    }
}
