using TextAnalyzer.Core.Model.Interfaces;
using TextAnalyzer.Core.Model.Symbols;
using TextAnalyzer.IO;

namespace TextAnalyzer.Core.Tasks;

public class TasksWorker(IText text, IOutput output)
{
    private readonly IText _text = text;
    private readonly IOutput _output = output;


    public void GetWordWithMaxNumberOfDigits()
    {
        var word = _text
            .SelectMany(s => s.OfType<IWord>())
            .Where(w => w.Any(s => s.Type == SymbolType.Digit))
            ?.MaxBy(w => w.Count(s => s.Type == SymbolType.Digit));

        _output.Print("");

        if (word == null)
        {
            _output.Print("Слово содержащих цифры в тексте нет");
        }
        else
        {
            _output.Print($"Слово содержащие максимальное количество цифр: {word}");
        }
    }

    public void GetMostLongestWordAndHowManyTimesItOccurs()
    {
        var word = _text
            .SelectMany(s => s.OfType<IWord>())
            .MaxBy(w => w.Count());

        var count = _text
            .SelectMany(s => s.OfType<IWord>())
            .Where(w => w == word)
            .Count();

        _output.Print("");
        _output.Print($"Самое длинное слово \"{word}\" встречается {count} раз.");
    }

    public void GetExclamationAndQuestionSentences()
    {
        _output.Print("");
        _output.Print("Вопросительные предложения");

        var questionSentences = _text
                .Where(s => s.LastOrDefault() is ISymbol symbol && symbol.Type == SymbolType.Question)
                .ToList();

        if (questionSentences.Count == 0)
        {
            _output.Print("В тексте нет вопросительных предложений");
        }
        else
        {
            _output.Print(string.Join(" ", questionSentences));
        }

        _output.Print("");
        _output.Print("Восклицательные предложения");


        var exclamationSentences = _text
                .Where(s => s.LastOrDefault() is ISymbol symbol && symbol.Type == SymbolType.Exclamation)
                .ToList();

        if (exclamationSentences.Count == 0)
        {
            _output.Print("В тексте нет восклицательных предложений");
        }
        else
        {
            _output.Print(string.Join(" ", exclamationSentences));
        }
    }

    public void GetSentencesWithoutCommas()
    {
        _output.Print("");
        _output.Print("Предложения без запятых");

        var sentencesWithoutCommas = _text
                .Where(s => !s.Any(se => se is ISymbol symbol && symbol.Equals(',')))
                .ToList();

        if (sentencesWithoutCommas.Count == 0)
        {
            _output.Print("В тексте нет предложений без запятых");
        }
        else
        {
            _output.Print(string.Join(" ", sentencesWithoutCommas));
        }
    }

    public void GetWordsWithSameBeginAndEnd()
    {
        _output.Print("");
        _output.Print("Слова с одинаковой первой и последней буквой");

        var words = _text
            .SelectMany(s => s.OfType<IWord>())
            .Where(w => w.Count() > 1 && w.First().Equals(w.Last()))
            .ToList();

        if (words.Count == 0)
        {
            _output.Print("Слова не найдены");
        }
        else
        {
            _output.Print(string.Join(", ", words));
        }
    }

    public void ExchangeNumbersToLetters()
    {
        var numbers = new Dictionary<char, string>()
        {
            { '0', "ноль" },
            { '1', "один" },
            { '2', "два" },
            { '3', "три" },
            { '4', "четыре" },
            { '5', "пять" },
            { '6', "шесть" },
            { '7', "семь" },
            { '8', "восемь" },
            { '9', "девять" },
        };

        _output.Print("");
        _output.Print("Заменить все цифры на их написание");

        foreach (var sentence in _text)
        {
            foreach (var sentenceElement in sentence)
            {
                if (sentenceElement is not IWord word || !word.Any(s => s.Type == SymbolType.Digit))
                    continue;

                for (var i = 0; i < word.Count(); i++)
                {
                    var symbol = word.ElementAt(i);
                    if (symbol.Type == SymbolType.Digit)
                    {
                        if (numbers.TryGetValue(symbol.SymbolChar!.Value, out var numberStr))
                            word.Replace(i, numberStr);
                    }
                }
            }
        }

        _output.Print(_text);
    }
}

