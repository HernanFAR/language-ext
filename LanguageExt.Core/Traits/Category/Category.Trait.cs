using LanguageExt.Traits;

namespace LanguageExt.Traits;

public interface Category<F>
    where F : Category<F>
{
    public static abstract K<F, A, A> Identity<A>();

    public static abstract K<F, A, C> Compose<A, B, C>(
        K<F, A, B> first,
        K<F, B, C> second);

    public static virtual K<F, A, D> Compose<A, B, C, D>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third) =>
        F.Compose(first, F.Compose(second, third));

    public static virtual K<F, A, E> Compose<A, B, C, D, E>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third,
        K<F, D, E> fourth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, fourth)));

    public static virtual K<F, A, G> Compose<A, B, C, D, E, G>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third,
        K<F, D, E> fourth,
        K<F, E, G> fifth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, F.Compose(fourth, fifth))));

    public static virtual K<F, A, H> Compose<A, B, C, D, E, G, H>(
        K<F, A, B> first,
        K<F, B, C> second,
        K<F, C, D> third,
        K<F, D, E> fourth,
        K<F, E, G> fifth,
        K<F, G, H> sixth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, F.Compose(fourth, F.Compose(fifth, sixth)))));
}
