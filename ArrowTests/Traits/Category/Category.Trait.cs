using LanguageExt.Traits;

namespace ArrowTests.Traits;

public interface Category<F>
    where F : Category<F>
{
    public static abstract K<F, A, A> Identity<A>();

    public static abstract K<F, I1, O> Compose<I1, I2, O>(
        K<F, I1, I2> first,
        K<F, I2, O> second);

    public static virtual K<F, I1, O> Compose<I1, I2, I3, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, O> third) =>
        F.Compose(first, F.Compose(second, third));

    public static virtual K<F, I1, O> Compose<I1, I2, I3, I4, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, O> fourth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, fourth)));

    public static virtual K<F, I1, O> Compose<I1, I2, I3, I4, I5, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, I5> fourth,
        K<F, I5, O> fifth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, F.Compose(fourth, fifth))));

    public static virtual K<F, I1, O> Compose<I1, I2, I3, I4, I5, I6, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, I5> fourth,
        K<F, I5, I6> fifth,
        K<F, I6, O> sixth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, F.Compose(fourth, F.Compose(fifth, sixth)))));
}
