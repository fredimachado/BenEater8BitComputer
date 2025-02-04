﻿using System.Collections.Immutable;

namespace BenEater8BitComputer.Compiler.Text;

/// <summary>
/// Source code based on Immo Landwerth's (@terrajobst) compiler series
/// https://www.youtube.com/channel/UCaFP8iQMTuPXinXBMEXsSuw
/// </summary>
public sealed class SourceText
{
    private readonly string text;

    private SourceText(string text)
    {
        this.text = text;
        Lines = ParseLines(this, text);
    }

    public ImmutableArray<TextLine> Lines { get; }

    public char this[int index] => text[index];

    public int Length => text.Length;

    public int GetLineIndex(int position)
    {
        var lower = 0;
        var upper = Lines.Length - 1;

        while (lower <= upper)
        {
            var index = lower + (upper - lower) / 2;
            var start = Lines[index].Start;

            if (position == start)
            {
                return index;
            }

            if (start > position)
            {
                upper = index - 1;
            }
            else
            {
                lower = index + 1;
            }
        }

        return lower - 1;
    }

    public static SourceText From(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return new SourceText(text);
    }

    public (int LineNumber, int Column) GetLineNumberAndColumn(TextSpan span)
    {
        var lineIndex = GetLineIndex(span.Start);
        var lineNumber = lineIndex + 1;
        var column = span.Start - Lines[lineIndex].Start + 1;

        return (lineNumber, column);
    }

    public override string ToString() => text;

    public string ToString(int start, int length) => text.Substring(start, length); 

    public string ToString(TextSpan span) => ToString(span.Start, span.Length);

    private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
    {
        var result = ImmutableArray.CreateBuilder<TextLine>();

        var position = 0;
        var lineStart = 0;

        while (position < text.Length)
        {
            var lineBreakWidth = GetLineBreakWidth(text, position);
            if (lineBreakWidth == 0)
            {
                position++;
            }
            else
            {
                AddLine(result, sourceText, position, lineStart, lineBreakWidth);

                position += lineBreakWidth;
                lineStart = position;
            }
        }

        if (position >= lineStart)
        {
            AddLine(result, sourceText, position, lineStart, 0);
        }

        return result.ToImmutable();
    }

    private static void AddLine(ImmutableArray<TextLine>.Builder result, SourceText sourceText, int position, int lineStart, int lineBreakWidth)
    {
        var lineLength = position - lineStart;
        var lineLengthIncludingLineBreak = lineLength + lineBreakWidth;
        var line = new TextLine(sourceText, lineStart, lineLength, lineLengthIncludingLineBreak);
        result.Add(line);
    }

    private static int GetLineBreakWidth(string text, int position)
    {
        var c = text[position];
        var l = position + 1 >= text.Length ? '\0' : text[position + 1];

        if (c == '\r' && l == '\n')
        {
            return 2;
        }

        else if (c == '\r' || c == '\n')
        {
            return 1;
        }

        return 0;
    }
}
