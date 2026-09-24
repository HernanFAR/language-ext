using System;
using System.Collections.Generic;
using LanguageExt.Common;
using LanguageExt.Traits;
using Xunit;

namespace LanguageExt.Tests.Transformer;

public class FreeTTests
{
    [Fact]
    public void FreeT_composes_free_instructions_with_base_monad_actions()
    {
        List<string> trace = [];

        var program =
            from first in FreeT.liftF<TraceF, Fin, int>(
                new TraceOp<int>("first", 10))
            from semantic in Fin.Succ(5)
            from second in FreeT.liftF<TraceF, Fin, int>(
                new TraceOp<int>("second", 3))
            select first + semantic + second;

        var result = Run(program, trace);

        Assert.Equal(18, result.ThrowIfFail());
        Assert.Equal(["first", "second"], trace);
    }

    [Fact]
    public void Base_monad_failure_short_circuits_later_free_instructions()
    {
        List<string> trace = [];

        var program =
            from first in FreeT.liftF<TraceF, Fin, int>(
                new TraceOp<int>("first", 10))
            from semantic in Fin.Fail<int>(
                Error.New("semantic failure"))
            from second in FreeT.liftF<TraceF, Fin, int>(
                new TraceOp<int>("second", 3))
            select first + semantic + second;

        var result = Run(program, trace);

        Assert.True(result.IsFail);
        Assert.Equal(["first"], trace);
    }

    [Fact]
    public void Existing_Free_program_can_be_lifted_without_rewriting_its_algebra()
    {
        List<string> trace = [];

        var free =
            from first in Free.lift<TraceF, int>(
                new TraceOp<int>("first", 4))
            from second in Free.lift<TraceF, int>(
                new TraceOp<int>("second", 6))
            select first + second;

        var program = FreeT.liftFree<TraceF, Fin, int>(free);

        var result = Run(program, trace);

        Assert.Equal(10, result.ThrowIfFail());
        Assert.Equal(["first", "second"], trace);
    }

    private static Fin<A> Run<A>(
        FreeT<TraceF, Fin, A> program,
        List<string> trace) =>
        program.runFreeT
            .As()
            .Bind(step =>
                step switch
                {
                    FreeTPure<TraceF, Fin, A>(var value) =>
                        Fin.Succ(value),

                    FreeTSuspend<TraceF, Fin, A>(var suspended) =>
                        suspended switch
                        {
                            TraceOp<FreeT<TraceF, Fin, A>>(var name, var next) =>
                                RunLogged(name, next, trace),

                            _ => throw new NotSupportedException()
                        },

                    _ => throw new NotSupportedException()
                });

    private static Fin<A> RunLogged<A>(
        string name,
        FreeT<TraceF, Fin, A> next,
        List<string> trace)
    {
        trace.Add(name);
        return Run(next, trace);
    }
}

public sealed record TraceOp<A>(
    string Name,
    A Value) : K<TraceF, A>;

public sealed class TraceF : Functor<TraceF>
{
    static K<TraceF, B> Functor<TraceF>.Map<A, B>(
        Func<A, B> f,
        K<TraceF, A> ma) =>
        ma switch
        {
            TraceOp<A>(var name, var value) =>
                new TraceOp<B>(name, f(value)),

            _ => throw new NotSupportedException()
        };
}
