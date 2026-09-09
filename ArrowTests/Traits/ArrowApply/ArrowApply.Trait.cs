using LanguageExt.Traits;

namespace ArrowTests.Traits;

public interface ArrowApply<F> : Arrow<F>
    where F : ArrowApply<F>
{
    static abstract K<F, (K<F, I, O> Arrow, I Input), O> Apply<I, O>();

    static virtual K<F, I, O> Bind<I, A, O>(
        K<F, I, A> first,
        Func<A, K<F, A, O>> next) =>
        F.Compose(
            first,
            F.Compose(
                F.Lift<A, (K<F, A, O> Arrow, A Input)>(
                    value => (next(value), value)),
                F.Apply<A, O>()));
}
