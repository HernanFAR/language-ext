using System;
using LanguageExt.Traits;

namespace LanguageExt;

/// <summary>
/// One observable layer of a free monad transformer.
/// </summary>
public abstract record FreeTStep<F, M, A>
    where F : Functor<F>
    where M : Monad<M>;

/// <summary>
/// Terminal layer of a free monad transformer.
/// </summary>
public sealed record FreeTPure<F, M, A>(A Value)
    : FreeTStep<F, M, A>
    where F : Functor<F>
    where M : Monad<M>;

/// <summary>
/// Suspended functor layer of a free monad transformer.
/// </summary>
public sealed record FreeTSuspend<F, M, A>(
    K<F, FreeT<F, M, A>> Value)
    : FreeTStep<F, M, A>
    where F : Functor<F>
    where M : Monad<M>;

/// <summary>
/// Free monad transformer.
///
/// The representation is:
///
///     M (Pure A | Suspend (F (FreeT F M A)))
///
/// which allows composition using only a Functor F and a Monad M.
/// </summary>
public sealed record FreeT<F, M, A>(
    K<M, FreeTStep<F, M, A>> runFreeT)
    : K<FreeT<F, M>, A>
    where F : Functor<F>
    where M : Monad<M>
{
    public FreeT<F, M, B> Map<B>(Func<A, B> f) =>
        Bind(value => FreeT.pure<F, M, B>(f(value)));

    public FreeT<F, M, B> Select<B>(Func<A, B> f) =>
        Map(f);

    public FreeT<F, M, B> Bind<B>(
        Func<A, FreeT<F, M, B>> f) =>
        new(
            M.Bind(
                runFreeT,
                step => step switch
                {
                    FreeTPure<F, M, A>(var value) =>
                        f(value).runFreeT,

                    FreeTSuspend<F, M, A>(var suspended) =>
                        M.Pure<FreeTStep<F, M, B>>(
                            new FreeTSuspend<F, M, B>(
                                F.Map(
                                    next => next.Bind(f),
                                    suspended))),

                    _ => throw new NotSupportedException()
                }));

    public FreeT<F, M, B> Bind<B>(
        Func<A, K<FreeT<F, M>, B>> f) =>
        Bind(value => f(value).As());

    public FreeT<F, M, B> Bind<B>(
        Func<A, K<M, B>> f) =>
        Bind(value => FreeT.lift<F, M, B>(f(value)));

    public FreeT<F, M, C> SelectMany<B, C>(
        Func<A, FreeT<F, M, B>> bind,
        Func<A, B, C> project) =>
        Bind(value =>
            bind(value)
                .Map(next => project(value, next)));

    public FreeT<F, M, C> SelectMany<B, C>(
        Func<A, K<FreeT<F, M>, B>> bind,
        Func<A, B, C> project) =>
        Bind(value =>
            bind(value)
                .As()
                .Map(next => project(value, next)));

    public FreeT<F, M, C> SelectMany<B, C>(
        Func<A, K<M, B>> bind,
        Func<A, B, C> project) =>
        Bind(value =>
            FreeT
                .lift<F, M, B>(bind(value))
                .Map(next => project(value, next)));
}
