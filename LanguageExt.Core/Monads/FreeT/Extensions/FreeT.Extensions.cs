using LanguageExt.Traits;

namespace LanguageExt;

public static partial class FreeTExtensions
{
    public static FreeT<F, M, A> As<F, M, A>(
        this K<FreeT<F, M>, A> ma)
        where F : Functor<F>
        where M : Monad<M> =>
        (FreeT<F, M, A>)ma;

    public static FreeT<F, M, A> ToFreeT<F, M, A>(
        this Free<F, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        FreeT.liftFree<F, M, A>(value);
}
