using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitRp.Core.Helpers.RegularExpressions;
using TwitRp.Core.Interfaces;
using TwitRp.Core.Models.Emoji;

namespace TwitRp.Services.Processors.Emoji
{
    public class EmojiProcessor : IEmojiProcessor,IProcessor
    {
        public ILogger Logger { get; set; }
        public EmojiProcessorOptions EmojiProcessorOptions { get; set; }

        public EmojiProcessor(IConfiguration configuration, ILogger<EmojiProcessor> logger)
        {
            EmojiProcessorOptions = configuration.GetSection(EmojiProcessorOptions.EmojiProcessor).Get<EmojiProcessorOptions>();
            Logger = logger;
        }
        public List<string> GetListOfEmojis(string inputString)
        {
            List<string> listOfEmojis = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(inputString))
                {
                    listOfEmojis.AddRange(RegexHelper.GetListOfMatchValues(inputString, EmojiProcessorOptions.EmojiRegexPattern));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error at GetListOfEmojis", ex);
            }
            return listOfEmojis;
        }

        public List<string> GetListOfEmojis(List<string> listOfInputStrings)
        {
            List<string> listOfEmojis = new List<string>();
            try
            {
                foreach (string inputString in listOfInputStrings)
                {
                    if (!string.IsNullOrEmpty(inputString))
                    {
                        listOfEmojis.AddRange(RegexHelper.GetListOfMatchValues(inputString, EmojiProcessorOptions.EmojiRegexPattern));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error at GetListOfEmojis", ex);
            }
            return listOfEmojis;
        }

        public Dictionary<string, int> GetEmojisWithCount(Dictionary<string, int> EmojisWithCount, List<string> listOfEmojis)
        {
            try
            {
                List<string> distinctEmojis = listOfEmojis.Distinct().ToList();
                if (distinctEmojis != null && distinctEmojis.Count > 0)
                {
                    foreach (string emoji in listOfEmojis)
                    {
                        List<string> listPerEmoji = listOfEmojis.Where(x => x == emoji).ToList();
                        int count = (listPerEmoji != null) ? listPerEmoji.Count : 0;
                        if (EmojisWithCount.ContainsKey(emoji))
                        {
                            EmojisWithCount[emoji] += count;
                        }
                        else
                        {
                            EmojisWithCount.Add(emoji, count);
                        }
                        EmojisWithCount = EmojisWithCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error at GetEmojisWithCount", ex);
            }
            return EmojisWithCount;
        }
    }
}
