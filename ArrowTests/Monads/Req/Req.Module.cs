using ArrowTests.Traits;
using LanguageExt.Traits;

namespace ArrowTests.Monads;

public partial class Req
{
    
}

public partial class Req<IN>
{
    
}

public partial class Req<IN, OUT>
{
    // Category 
    public static Req<IN, OUT, I, O> Compose<I, I2, O>(
        K<Req<IN, OUT>, I, I2> m1,
        K<Req<IN, OUT>, I2, O> m2) =>
        Category.Compose(m1, m2).AsBi();

    public static Req<IN, OUT, I, O> Compose<I, I2, I3, O>(
        K<Req<IN, OUT>, I, I2> m1,
        K<Req<IN, OUT>, I2, I3> m2,
        K<Req<IN, OUT>, I3, O> m3) =>
        Category.Compose(m1, m2, m3).AsBi();

    public static Req<IN, OUT, I, O> Compose<I, I2, I3, I4, O>(
        K<Req<IN, OUT>, I, I2> m1,
        K<Req<IN, OUT>, I2, I3> m2,
        K<Req<IN, OUT>, I3, I4> m3,
        K<Req<IN, OUT>, I4, O> m4) =>
        Category.Compose(m1, m2, m3, m4).AsBi();

    public static Req<IN, OUT, I, O> Compose<I, I2, I3, I4, I5, O>(
        K<Req<IN, OUT>, I, I2> m1,
        K<Req<IN, OUT>, I2, I3> m2,
        K<Req<IN, OUT>, I3, I4> m3,
        K<Req<IN, OUT>, I4, I5> m4,
        K<Req<IN, OUT>, I5, O> m5) =>
        Category.Compose(m1, m2, m3, m4, m5).AsBi();

    public static Req<IN, OUT, I, O> Compose<I, I2, I3, I4, I5, I6, O>(
        K<Req<IN, OUT>, I, I2> m1,
        K<Req<IN, OUT>, I2, I3> m2,
        K<Req<IN, OUT>, I3, I4> m3,
        K<Req<IN, OUT>, I4, I5> m4,
        K<Req<IN, OUT>, I5, I6> m5,
        K<Req<IN, OUT>, I6, O> m6) =>
        Category.Compose(m1, m2, m3, m4, m5, m6).AsBi();

    // Arrow
    public static Req<IN, OUT, I, O> Lift<I, O>(Func<I, O> f) =>
        Req<IN, OUT, I>.Lift(f);

    // ArrowApply
    public static Req<IN, OUT, I1, O> Bind<I1, I2, O>(
        K<Req<IN, OUT>, I1, I2> ma,
        Func<I2, K<Req<IN, OUT>, I2, O>> fb) =>
        ArrowApply.Bind(ma, fb).AsBi();
}

public partial class Req<IN, OUT, I>
{    
    // Category
    public static Req<IN, OUT, I, O> Compose<I2, O>(
        K<Req<IN, OUT>, I, I2> m1, 
        K<Req<IN, OUT>, I2, O> m2) => 
        Req<IN, OUT>.Compose(m1, m2);
    
    public static Req<IN, OUT, I, O> Compose<I2, O>(
        K<Req<IN, OUT>, I, I2> m1, 
        Req<IN, OUT, I2, O> m2) => 
        Req<IN, OUT>.Compose(m1, m2);

    public static Req<IN, OUT, I, O> Compose<I2, I3, O>(
        K<Req<IN, OUT>, I, I2> m1, 
        K<Req<IN, OUT>, I2, I3> m2, 
        K<Req<IN, OUT>, I3, O> m3) => 
        Req<IN, OUT>.Compose(m1, m2, m3);

    public static Req<IN, OUT, I, O> Compose<I2, I3, I4, O>(
        K<Req<IN, OUT>, I, I2> m1, 
        K<Req<IN, OUT>, I2, I3> m2, 
        K<Req<IN, OUT>, I3, I4> m3, 
        K<Req<IN, OUT>, I4, O> m4) => 
        Req<IN, OUT>.Compose(m1, m2, m3, m4);

    public static Req<IN, OUT, I, O> Compose<I2, I3, I4, I5, O>(
        K<Req<IN, OUT>, I, I2> m1, 
        K<Req<IN, OUT>, I2, I3> m2, 
        K<Req<IN, OUT>, I3, I4> m3, 
        K<Req<IN, OUT>, I4, I5> m4, 
        K<Req<IN, OUT>, I5, O> m5) => 
        Req<IN, OUT>.Compose(m1, m2, m3, m4, m5);

    public static Req<IN, OUT, I, O> Compose<I2, I3, I4, I5, I6, O>(
        K<Req<IN, OUT>, I, I2> m1, 
        K<Req<IN, OUT>, I2, I3> m2, 
        K<Req<IN, OUT>, I3, I4> m3, 
        K<Req<IN, OUT>, I4, I5> m4, 
        K<Req<IN, OUT>, I5, I6> m5, 
        K<Req<IN, OUT>, I6, O> m6) => 
        Req<IN, OUT>.Compose(m1, m2, m3, m4, m5, m6);

    // Arrow
    public static Req<IN, OUT, I, O> Lift<O>(Func<I, O> f) =>
        Arrow.Lift<Req<IN, OUT>, I, O>(f).AsBi();
        
    // ArrowApply
    public static Req<IN, OUT, I, O> Bind<I2, O>(
        K<Req<IN, OUT>, I, I2> ma,
        Func<I2, K<Req<IN, OUT>, I2, O>> fb) =>
        Req<IN, OUT>.Bind(ma, fb);
}