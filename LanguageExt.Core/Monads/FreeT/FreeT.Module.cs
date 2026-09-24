using LanguageExt.Traits;

namespace LanguageExt;

public static class FreeT
{
    /// <summary>
    /// Lift a pure value into FreeT.
    /// </summary>
    public static FreeT<F, M, A> pure<F, M, A>(A value)
        where F : Functor<F>
        where M : Monad<M> =>
        new(
            M.Pure<FreeTStep<F, M, A>>(
                new FreeTPure<F, M, A>(value)));

    /// <summary>
    /// Lift an action from the base monad M.
    /// </summary>
    public static FreeT<F, M, A> lift<F, M, A>(
        K<M, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        new(
            M.Map<A, FreeTStep<F, M, A>>(
                item => new FreeTPure<F, M, A>(item),
                value));

    /// <summary>
    /// Lift one instruction from the free functor F.
    /// </summary>
    public static FreeT<F, M, A> liftF<F, M, A>(
        K<F, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        new(
            M.Pure<FreeTStep<F, M, A>>(
                new FreeTSuspend<F, M, A>(
                    F.Map(
                        item => pure<F, M, A>(item),
                        value))));

    /// <summary>
    /// Lift an existing Free program into FreeT without changing its algebra.
    /// </summary>
    public static FreeT<F, M, A> liftFree<F, M, A>(
        Free<F, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        value switch
        {
            Pure<F, A>(var item) =>
                pure<F, M, A>(item),

            Bind<F, A>(var suspended) =>
                new FreeT<F, M, A>(
                    M.Pure<FreeTStep<F, M, A>>(
                        new FreeTSuspend<F, M, A>(
                            F.Map(
                                next => liftFree<F, M, A>(next),
                                suspended)))),

            _ => throw new NotSupportedException()
        };
}
