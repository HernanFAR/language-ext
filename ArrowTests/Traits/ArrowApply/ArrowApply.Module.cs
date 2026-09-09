using LanguageExt.Traits;

namespace ArrowTests.Traits;
    
public static class ArrowApply
{
    public static K<F, (K<F, I, O> Arrow, I Input), O> Apply<F, I, O>() 
        where F : ArrowApply<F> =>
        F.Apply<I, O>();

    public static K<F, I, O> Bind<F, I, I2, O>(K<F, I, I2> ma, Func<I2, K<F, I2, O>> fb) 
        where F : ArrowApply<F> =>
        F.Bind(ma, fb);

}
