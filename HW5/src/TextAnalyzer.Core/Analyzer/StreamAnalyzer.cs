using System.Globalization;
using System.Text;
using TextAnalyzer.Core.Analyzer.StateMachine;
using TextAnalyzer.Core.Loggers;
using TextAnalyzer.Core.Model;
using TextAnalyzer.Core.Model.Interfaces;
using TextAnalyzer.Core.Model.Symbols;
using TextAnalyzer.Core.Model.Symbols.OneSign;


namespace TextAnalyzer.Core.Analyzer;

public class StreamAnalyzer : IAnalyzer
{
    private readonly ILogger? _logger;
    private readonly Stream _stream;


    public StreamAnalyzer(Stream stream, ILogger logger)
    {
        _logger = logger;
        _stream = stream;
    }

    public StreamAnalyzer(Stream stream)
    {
        _logger = null;
        _stream = stream;
    }

    public IText Analyze()
    {
        var buffer = new AnalyzerBuffer();

        var stateMachine = new StateMachine.StateMachine(buffer);

        using (var reader = new StreamReader(_stream, Encoding.Default))
        {
            while (reader.Peek() != -1)
            {
                var charBuffer = new char[4096];

                var readLength = reader.Read(charBuffer, 0, charBuffer.Length);

                for (var i = 0; i < readLength; i++)
                {
                    var nextSymbol = GetSymbol(charBuffer[i]);

                    try
                    {
                        stateMachine.MoveNext(nextSymbol)?.Invoke(nextSymbol);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        _logger?.Log(e.Message);
                    }
                    catch (StateMachineException e)
                    {
                        _logger?.Log(e.Message);
                    }
                }
            }
        }

        try
        {
            stateMachine.MoveNext(new EndSymbol()).Invoke(new EndSymbol());
        }
        catch (ArgumentOutOfRangeException e)
        {
            _logger?.Log(e.Message);
        }
        catch (StateMachineException e)
        {
            _logger?.Log(e.Message);
        }

        if (buffer.Sentences.Count != 0)
        {
            _logger?.Log($"Сериализовано {buffer.Sentences.Count} предложений");
            return new Text(buffer.Sentences);
        }

        throw new ArgumentException("Stream didn't serialized");
    }

    private static ISymbol GetSymbol(char c)
    {
        ISymbol symbol = char.GetUnicodeCategory(c) switch
        {
            UnicodeCategory.UppercaseLetter
                or UnicodeCategory.LowercaseLetter => new Letter(c),
            UnicodeCategory.DecimalDigitNumber => new Digit(c),
            UnicodeCategory.InitialQuotePunctuation
                or UnicodeCategory.FinalQuotePunctuation
                or UnicodeCategory.OpenPunctuation
                or UnicodeCategory.ClosePunctuation
                or UnicodeCategory.DashPunctuation
                or UnicodeCategory.OtherPunctuation
                or UnicodeCategory.ConnectorPunctuation => c switch
                {
                    '.' => new Dot(),
                    '!' => new Exclamation(),
                    '?' => new Question(),
                    _ => new PunctuationMark(c)
                },
            UnicodeCategory.SpaceSeparator
                or UnicodeCategory.Control => new Space(),
            _ => new Undefined(c),
        };
        return symbol;
    }

    public void Dispose()
    {
        _stream?.Dispose();
    }
}

