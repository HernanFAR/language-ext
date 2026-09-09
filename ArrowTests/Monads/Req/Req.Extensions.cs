using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace ArrowTests.Monads;

public static class ReqKindExtensions
{

    extension<IN, OUT, I, O>(K<Req<IN, OUT, I>, O> m)
    {
        public Req<IN, OUT, I, O> As() =>
            (Req<IN, OUT, I, O>)m;

        public Either<Error, ReqState<O>> RawRun(IN input, Either<Error, ReqState<I>> previous) =>
            m.As().RawRun(input, previous);
    }

    extension<IN, OUT, I, O>(K<Req<IN, OUT>, I, O> m)
    {
        public Req<IN, OUT, I, O> AsBi() =>
            (Req<IN, OUT, I, O>)m;

        public Either<Error, ReqState<O>> RawRunBi(IN input, Either<Error, ReqState<I>> previous) =>
            m.AsBi().RawRun(input, previous);
        

    }

    extension<IN, OUT, I, O>(K<Req<IN, OUT>, I, O>)
    {
        public static Req<IN, OUT, I, O> operator +(
            K<Req<IN, OUT>, I, O> ma) =>
            ma.AsBi();
    }

    extension<IN, OUT, I, O, FinO>(K<Req<IN, OUT>, I, O>)
    {
        public static Req<IN, OUT, I, FinO> operator >>(
            K<Req<IN, OUT>, I, O> ma,
            K<Req<IN, OUT>, O, FinO> mb) =>
            Req<IN, OUT>.Compose(ma, mb);

        public static Req<IN, OUT, I, FinO> operator *(
            K<Req<IN, OUT>, I, O> ma,
            Func<O, FinO> fb) =>
            ma >> Req<IN, OUT>.Transform(fb);

    }

    public static Fin<OUT> RunFin<IN, OUT>(this Req<IN, OUT, IN, OUT> ma, IN input) =>
        ma.RawRun(input, ReqState.New(input))
          .Match(
              Left: Fin.Fail<OUT>,
              Right: r => r switch
              {
                  (_, { IsEmpty: false } e) => Fin.Fail<OUT>(e),
                  var (v, _) => Fin.Succ(v)
              });
}
