using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using static LanguageExt.Prelude;

namespace ArrowTests.Monads;

public static class ReqState
{
    public static ReqState<O> New<O>(O v) =>
        New(v, Error.Empty);

    public static ReqState<O> New<O>(O v, Error e) =>
        new(v, e);

    public static ReqState<Unit> Unit(Error e) =>
        new(unit, e);
}

public readonly record struct ReqState<O>(O Value, Error Error)
{
    public bool IsValid => Error.IsEmpty;
    
    public ReqState<O2> Map<O2>(Func<O, O2> f) =>
        new(f(Value), Error);

    public ReqState<O> MapError(Func<Error, Error> f) =>
        this with { Error = f(Error) };

    public ReqState<O2> Bind<O2>(Func<O, ReqState<O2>> f) =>
        f(Value);

    public ReqState<(O, Error)> Express()
    {
        var response = this;
        return Map(v => (v, response.Error));
    }
}

// Implementación del monad
public sealed record Req<IN, OUT, I, O>(Func<IN, Either<Error, ReqState<I>>, Either<Error, ReqState<O>>> RawRunF) :
    K<Req<IN, OUT>, I, O>,
    K<Req<IN, OUT, I>, O>
{
    private Func<IN, Either<Error, ReqState<I>>, Either<Error, ReqState<O>>> RawRunF { get; } = RawRunF;
    
    public Either<Error, ReqState<O>> RawRun(IN input, Either<Error, ReqState<I>> previous) => 
        RawRunF(input, previous);

    public Req<IN, OUT, I, FinO> Compose<FinO>(
        K<Req<IN, OUT>, O, FinO> m2) =>
        Req<IN, OUT, I>.Compose(this, m2);

    public Req<IN, OUT, I, FinO> Compose<O2, FinO>(
        K<Req<IN, OUT>, O, O2> m2,
        K<Req<IN, OUT>, O2, FinO> m3) =>
        Req<IN, OUT, I>.Compose(this, m2, m3);

    public Req<IN, OUT, I, FinO> Compose<O2, O3, FinO>(
        K<Req<IN, OUT>, O, O2> m2,
        K<Req<IN, OUT>, O2, O3> m3,
        K<Req<IN, OUT>, O3, FinO> m4) =>
        Req<IN, OUT, I>.Compose(this, m2, m3, m4);

    public Req<IN, OUT, I, FinO> Compose<I2, I3, I4, FinO>(
        K<Req<IN, OUT>, O, I2> m2,
        K<Req<IN, OUT>, I2, I3> m3,
        K<Req<IN, OUT>, I3, I4> m4,
        K<Req<IN, OUT>, I4, FinO> m5) =>
        Req<IN, OUT, I>.Compose(this, m2, m3, m4, m5);

    public Req<IN, OUT, I, FinO> Bind<FinO>(Func<O, Req<IN, OUT, O, FinO>> f) =>
        Req<IN, OUT, I>.Bind(this, f);

    public Req<IN, OUT, I, FinO> Compose<O2, O3, O4, O5, FinO>(
        K<Req<IN, OUT>, O, O2> m2,
        K<Req<IN, OUT>, O2, O3> m3,
        K<Req<IN, OUT>, O3, O4> m4,
        K<Req<IN, OUT>, O4, O5> m5,
        K<Req<IN, OUT>, O5, FinO> m6) =>
        Req<IN, OUT, I>.Compose(this, m2, m3, m4, m5, m6);

    public static implicit operator Req<IN, OUT, I, O>(Pure<O> mf) =>
        Req<IN, OUT, I>.Accept(mf.Value);
}
