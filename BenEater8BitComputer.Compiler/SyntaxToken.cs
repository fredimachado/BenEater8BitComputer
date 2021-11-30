﻿using BenEater8BitComputer.Compiler.Text;

namespace BenEater8BitComputer.Compiler;

public sealed class SyntaxToken
{
    public SyntaxToken(SyntaxKind kind, int position, string text, object value)
    {
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }

    public SyntaxKind Kind { get; }
    public int Position { get; }
    public string Text { get; }
    public object Value { get; }
    public TextSpan Span => new TextSpan(Position, Text.Length);
}
