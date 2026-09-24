using System;
using LanguageExt.Traits;

namespace LanguageExt;

/// <summary>
/// HKT witness and type-class implementation for FreeT.
/// </summary>
public sealed class FreeT<F, M> :
    MonadT<FreeT<F, M>, M>
    where F : Functor<F>
    where M : Monad<M>
{
    static K<FreeT<F, M>, B> Monad<FreeT<F, M>>.Bind<A, B>(
        K<FreeT<F, M>, A> ma,
        Func<A, K<FreeT<F, M>, B>> f) =>
        ma.As().Bind(f);

    static K<FreeT<F, M>, B> Monad<FreeT<F, M>>.Recur<A, B>(
        A value,
        Func<A, K<FreeT<F, M>, Next<A, B>>> f) =>
        Monad.unsafeRecur(value, f);

    static K<FreeT<F, M>, B> Functor<FreeT<F, M>>.Map<A, B>(
        Func<A, B> f,
        K<FreeT<F, M>, A> ma) =>
        ma.As().Map(f);

    static K<FreeT<F, M>, A> Applicative<FreeT<F, M>>.Pure<A>(
        A value) =>
        FreeT.pure<F, M, A>(value);

    static K<FreeT<F, M>, B> Applicative<FreeT<F, M>>.Apply<A, B>(
        K<FreeT<F, M>, Func<A, B>> mf,
        K<FreeT<F, M>, A> ma) =>
        mf.As().Bind(
            f => ma.As().Map(f));

    static K<FreeT<F, M>, B> Applicative<FreeT<F, M>>.Apply<A, B>(
        K<FreeT<F, M>, Func<A, B>> mf,
        Memo<FreeT<F, M>, A> ma) =>
        mf.As().Bind(
            f => ma.Value.As().Map(f));

    static K<FreeT<F, M>, A> MonadT<FreeT<F, M>, M>.Lift<A>(
        K<M, A> ma) =>
        FreeT.lift<F, M, A>(ma);
}
