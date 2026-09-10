using LanguageExt.Traits;

namespace LanguageExt.Traits;

public static class Category
{
    public static K<F, A, A> Identity<F, A>()
        where F : Category<F> =>
        F.Identity<A>();

    public static K<F, I1, O> Compose<F, I1, I2, O>(
        K<F, I1, I2> first,
        K<F, I2, O> second)
        where F : Category<F> =>
        F.Compose(first, second);

    public static K<F, I1, O> Compose<F, I1, I2, I3, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, O> third)
        where F : Category<F> =>
        F.Compose(first, second, third);

    public static K<F, I1, O> Compose<F, I1, I2, I3, I4, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, O> fourth)
        where F : Category<F> =>
        F.Compose(first, second, third, fourth);

    public static K<F, I1, O> Compose<F, I1, I2, I3, I4, I5, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, I5> fourth,
        K<F, I5, O> fifth)
        where F : Category<F> =>
        F.Compose(first, second, third, fourth, fifth);

    public static K<F, I1, O> Compose<F, I1, I2, I3, I4, I5, I6, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, I5> fourth,
        K<F, I5, I6> fifth,
        K<F, I6, O> sixth)
        where F : Category<F> =>
        F.Compose(first, second, third, fourth, fifth, sixth);

}
