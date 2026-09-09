using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using static LanguageExt.Prelude;

namespace ArrowTests.Monads;

public partial class Req
{
    public static readonly Pure<Unit> Ok = Pure(unit);
}

public partial class Req<IN, OUT>
{
    public static readonly Req<IN, OUT, Unit, IN> Input =
        Readable.ask<Req<IN, OUT, Unit>, IN>().As();
    
    public static Req<IN, OUT, Unit, O> Accept<O>(O value) =>
        Req<IN, OUT, Unit>.Accept(value);

    public static Req<IN, OUT, Unit, Unit> Write(Error error) =>
        Req<IN, OUT, Unit>.Write(error);

    public static Req<IN, OUT, Unit, Unit> Write(string e) => 
        Req<IN, OUT, Unit>.Write(e);

    public static Req<IN, OUT, I, bool> Check<I>(Func<I, bool> f) =>
        Req<IN, OUT, I>.Check(f); 

    public static Req<IN, OUT, I, Unit> Prescribe<I>(Func<I, Error> f) =>
        Req<IN, OUT, I>.Prescribe(f);
        
    public static Req<IN, OUT, I, I> Ensure<I>(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Req<IN, OUT, I>.Ensure(fa, Fail);
        
    public static Req<IN, OUT, I, I> Ensure<I>(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Req<IN, OUT, I>.Ensure(fa, Fail);
        
    public static Req<IN, OUT, I, I> Ensure<I>(
        Func<I, bool> fa, 
        string Fail) =>
        Req<IN, OUT, I>.Ensure(fa, Fail);
                
    public static Req<IN, OUT, I, I> Avoid<I>(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Req<IN, OUT, I>.Avoid(fa, Fail);
                
    public static Req<IN, OUT, I, I> Avoid<I>(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Req<IN, OUT, I>.Avoid(fa, Fail);
                
    public static Req<IN, OUT, I, I> Avoid<I>(
        Func<I, bool> fa, 
        string Fail) =>
        Req<IN, OUT, I>.Avoid(fa, Fail);

    public static Req<IN, OUT, I, O> Transform<I, O>(Func<I, O> f) =>
        new((_, previous) => 
            previous.Bind<ReqState<O>>(p => p.IsValid ? p.Map(f) : p.Error));
}

public partial class Req<IN, OUT, I>
{
    public static readonly Req<IN, OUT, I, I> Identity =
        Category.Identity<Req<IN, OUT>, I>().AsBi();

    public static Req<IN, OUT, I, O> Accept<O>(O value) =>
        Arrow.Pure<Req<IN, OUT>, I, O>(value).AsBi();

    public static Req<IN, OUT, I, Unit> Write(Error error) =>
        Writable.tell<Req<IN, OUT, I>, Error>(error).As();

    public static Req<IN, OUT, I, Unit> Write(string e) => 
        Write(Error.New(e));

    public static Req<IN, OUT, I, bool> Check(Func<I, bool> f) =>
        Lift(f);

    public static Req<IN, OUT, I, Unit> Prescribe(Func<I, Error> f) =>
        Lift(f).Bind(Req<IN, OUT, Error>.Write);
        
    public static Req<IN, OUT, I, I> Ensure(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Identity.Bind(
            i => Compose(
                Identity, 
                fa(i) ? Req.Ok : Prescribe(Fail), 
                Req<IN, OUT, Unit>.Lift(_ => i)));
        
    public static Req<IN, OUT, I, I> Ensure(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Ensure(fa, i => Error.New(Fail(i)));
        
    public static Req<IN, OUT, I, I> Ensure(
        Func<I, bool> fa, 
        string Fail) =>
        Ensure(fa, _ => Fail);
                
    public static Req<IN, OUT, I, I> Avoid(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Ensure(i => !fa(i), Fail);
                
    public static Req<IN, OUT, I, I> Avoid(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Avoid(fa, i => Error.New(Fail(i)));
                
    public static Req<IN, OUT, I, I> Avoid(
        Func<I, bool> fa, 
        string Fail) =>
        Avoid(fa, i => Fail);
}