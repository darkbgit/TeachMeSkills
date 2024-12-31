using System.Text;
using System.Text.RegularExpressions;

namespace Calculator.Core;

public class EquationSolver
{
    private static readonly Dictionary<char, int> operatorPriority = new() {
        { '(', 0 },
        { '+', 1 },
        { '-', 1 },
        { '*', 2 },
        { '/', 2 },
        { '%', 3 },
        { 's', 4 },
        { '~', 5 }
    };

    public static double SolveEquation(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ParserException("Equation is empty.");

        ValidateExpression(expression);

        expression = NormalizeExpression(expression);

        var postfixExpr = ToPostfixExpression(expression);

        Console.WriteLine(postfixExpr);

        var stack = new Stack<double>();

        for (int i = 0; i < postfixExpr.Length; i++)
        {
            char c = postfixExpr[i];

            if (char.IsDigit(c))
            {
                var numberStr = ParseStringNumber(postfixExpr, ref i);
                if (double.TryParse(numberStr, out var number))
                {
                    stack.Push(number);
                }
                else
                {
                    throw new ParserException("Invalid number", i);
                }
            }
            else if (c == 's' || c == '~')
            {
                double first = stack.Count > 0 ? stack.Pop() : 0;
                stack.Push(DoOperation(c, first));
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                double second = stack.Count > 0 ? stack.Pop() : 0;
                double first = stack.Count > 0 ? stack.Pop() : 0;

                stack.Push(DoOperation(c, first, second));
            }
            else if (c == '%')
            {
                double second = stack.Count > 0 ? stack.Pop() : 0;
                double first = stack.Count > 0 ? stack.Peek() : 0;

                stack.Push(DoPercent(first, second, NextSymbol(postfixExpr, i + 1)));
            }
        }

        return stack.Pop();
    }

    private static string ToPostfixExpression(string expression)
    {
        string result = string.Empty;
        var stack = new Stack<char>();

        for (int i = 0; i < expression.Length; i++)
        {
            var c = expression[i];

            if (char.IsDigit(c) || c == '.')
            {
                if (!IsPreviousSymbolOperator(expression, i))
                    throw new ParserException($"Numbers allowed only after operators.", i);
                result += ParseStringNumber(expression, ref i) + " ";
            }
            else if (c == '(')
            {
                if (!IsPreviousSymbolOperator(expression, i))
                    throw new ParserException($"{c} allowed only after operators.", i);
                stack.Push(c);
            }
            else if (c == ')')
            {
                if (IsPreviousSymbolOperator(expression, i))
                    throw new ParserException($"{c} allowed only after numbers.", i);
                while (stack.Count > 0 && stack.Peek() != '(')
                    result += stack.Pop();

                stack.Pop();
            }
            else if (IsOperator(c))
            {
                var op = c;

                if (op == '%' && IsPreviousSymbolOperator(expression, i))
                    throw new ParserException($"{op} operator allowed only after numbers.", i);
                else if (op == '-' && IsPreviousSymbolOperator(expression, i))
                    op = '~';

                while (stack.Count > 0 && (operatorPriority[stack.Peek()] >= operatorPriority[op]))
                    result += stack.Pop();

                stack.Push(op);
            }
        }
        foreach (char op in stack)
            result += op;

        return result;
    }

    private static string ParseStringNumber(string expression, ref int index)
    {
        var numberStr = new StringBuilder();
        var isDotExist = false;

        for (; index < expression.Length; index++)
        {
            char c = expression[index];

            if (char.IsDigit(c))
            {
                numberStr.Append(c);
            }
            else if (c == '.' && !isDotExist)
            {
                if (numberStr.Length == 0)
                {
                    numberStr.Append('0');
                }

                numberStr.Append(c);
                isDotExist = true;
            }
            else if (c == '-' && isDotExist)
            {
                throw new ParserException("Invalid number", index);
            }
            else
            {
                index--;
                break;
            }
        }

        return numberStr.ToString();
    }

    private static bool IsPreviousSymbolOperator(string expression, int pos)
    {
        if (pos == 0)
            return true;

        while (pos > 0 && expression[pos - 1] == ' ')
            pos--;

        return IsOperator(expression[pos - 1]);
    }

    private static char NextSymbol(string expression, int pos)
    {
        while (pos < expression.Length && expression[pos] == ' ')
            pos++;

        return pos < expression.Length ? expression[pos] : '\0';
    }

    private static bool IsOperator(char c) => operatorPriority.ContainsKey(c);

    private static double DoOperation(char op, double first, double second)
    {
        return op switch
        {
            '+' => first + second,
            '-' => first - second,
            '*' => first * second,
            '/' => DoDivide(first, second),
            _ => throw new SolveException("Unknown operator type."),
        };
    }

    private static double DoOperation(char op, double first)
    {
        return op switch
        {
            '~' => -first,
            's' => DoSqrt(first),
            _ => throw new SolveException("Unknown operator type."),
        };
    }

    private static double DoSqrt(double first)
    {
        if (first < 0)
            throw new SolveException("Calculator doesn't support complex numbers.");

        return Math.Sqrt(first);
    }

    private static double DoDivide(double first, double second)
    {
        if (second == 0)
            throw new SolveException("Divide by 0 impossible.");

        return first / second;
    }

    private static double DoPercent(double first, double second, char nextOperator)
    {
        if (nextOperator == '+' || nextOperator == '-')
            return first * second / 100;
        else if (nextOperator == '*' || nextOperator == '/')
            return second / 100;

        return first * second / 100;
    }

    private static void ValidateExpression(string expression)
    {
        ValidateSymbols(expression);
        ValidateSymbolsSequence(expression);
        ValidateParentheses(expression);
    }

    public static void ValidateSymbols(string expression)
    {
        string regexPattern = @"\bsqrt\b|[^(?:\d|\+|\-|\*|\/|\(|\)|\.|\,\% )]";
        var regex = new Regex(regexPattern)
            .Matches(expression)
            .FirstOrDefault(m => m.Success && m.Value != "sqrt");

        if (regex != null)
        {
            throw new ParserException("Invalid symbol", regex.Index);
        }
    }

    public static void ValidateSymbolsSequence(string expression)
    {
        string pattern = @"^(\s*(sqrt\s*\(\s*\d+(\.\d+)?\s*\)|\d+(\.\d+)?)(\s*[-+*/]\s*(sqrt\s*\(\s*\d+(\.\d+)?\s*\)|\d+(\.\d+)?))*\s*)$";
        var regex = new Regex(pattern).Match(expression);
        if (regex.Success)
        {
            throw new ParserException("Invalid sequence");
        }
    }

    private static string NormalizeExpression(string expression) =>
        expression
            .Replace(',', '.')
            .Replace("sqrt", "s   ");

    private static void ValidateParentheses(string expression)
    {
        int balance = 0;
        var openParenthesisPositions = new Stack<int>();

        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];
            if (c == '(')
            {
                openParenthesisPositions.Push(i);
                balance++;
            }
            else if (c == ')')
            {
                if (--balance < 0)
                    throw new ParserException("Invalid parentheses", i);

                openParenthesisPositions.Pop();
            }
        }

        if (balance != 0)
        {
            throw new ParserException("Invalid parentheses", openParenthesisPositions.Pop());
        }
    }
}

