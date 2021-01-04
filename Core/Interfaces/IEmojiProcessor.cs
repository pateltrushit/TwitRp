using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Interfaces
{
    public interface IEmojiProcessor
    {
        List<string> GetListOfEmojis(string inputString);
        List<string> GetListOfEmojis(List<string> listOfInputStrings);
        Dictionary<string, int> GetEmojisWithCount(Dictionary<string, int> EmojisWithCount, List<string> listOfEmojis);
    }
}
