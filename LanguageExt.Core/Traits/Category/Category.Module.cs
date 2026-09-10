using LanguageExt.Traits;

namespace LanguageExt.Traits;

public static class Category
{
    public static K<F, A, A> identity<F, A>()
        where F : Category<F> =>
        F.Identity<A>();

    public static K<F, A, C> compose<F, A, B, C>(
        K<F, A, B> first,
        K<F, B, C> second)
        where F : Category<F> =>
        F.Compose(first, second);

    public static K<F, A, D> compose<F, A, B, C, D>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third)
        where F : Category<F> =>
        F.Compose(first, second, third);

    public static K<F, A, E> compose<F, A, B, C, D, E>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third,
        K<F, D, E> fourth)
        where F : Category<F> =>
        F.Compose(first, second, third, fourth);

    public static K<F, A, G> compose<F, A, B, C, D, E, G>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third,
        K<F, D, E> fourth,
        K<F, E, G> fifth)
        where F : Category<F> =>
        F.Compose(first, second, third, fourth, fifth);

    public static K<F, A, H> compose<F, A, B, C, D, E, G, H>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third,
        K<F, D, E> fourth,
        K<F, E, G> fifth,
        K<F, G, H> sixth)
        where F : Category<F> =>
        F.Compose(first, second, third, fourth, fifth, sixth);
}
