﻿using BenEater8BitComputer.Compiler.Text;
using System.Collections.Immutable;

namespace BenEater8BitComputer.Compiler;

public sealed class Program
{
    public Program(SourceText text, ImmutableArray<Diagnostic> diagnostics, ImmutableArray<InstructionSyntax> instructions, SyntaxToken endOfFileToken)
    {
        Text = text;
        Diagnostics = diagnostics;
        Instructions = instructions;
        EndOfFileToken = endOfFileToken;
    }

    public SourceText Text { get; }
    public ImmutableArray<Diagnostic> Diagnostics { get; }
    public ImmutableArray<InstructionSyntax> Instructions { get; }
    public SyntaxToken EndOfFileToken { get; }

    public static Program Parse(string text)
    {
        var sourceText = SourceText.From(text);
        return Parse(sourceText);
    }

    public static Program Parse(SourceText text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }
}
