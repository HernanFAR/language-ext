using LanguageExt.Traits;

namespace ArrowTests.Monads;

public static class ReqLawTesting
{
    public static void TestLaws()
    {
        Console.WriteLine("Experimentando Laws...");

        // Category + Arrow laws
        static bool Equivalent<IN, OUT, I, O>(
            K<Req<IN, OUT>, I, O> left,
            K<Req<IN, OUT>, I, O> right,
            IN input,
            I previous)
        {
            var initial = ReqState.New(previous);

            var leftResult =
                left.AsBi().RawRun(
                    input,
                    initial);

            var rightResult =
                right.AsBi().RawRun(
                    input,
                    initial);

            return leftResult == rightResult;
        }

        Console.WriteLine("Probando Category laws...");

        var lawIntToString =
            Arrow.Lift<Req<int, string>, int, string>(value => $"VALUE:{value}");

        var lawStringToInt =
            Arrow.Lift<Req<int, string>, string, int>(value => value.Length);

        var lawIntToBool =
            Arrow.Lift<Req<int, string>, int, bool>(value => value > 0);

        CategoryLaws.LeftIdentity(
                lawIntToString,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: 5));

        Console.WriteLine("Category.LeftIdentity OK");

        CategoryLaws.RightIdentity<
            Req<int, string>,
            int,
            string>(
                lawIntToString,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: 5));

        Console.WriteLine("Category.RightIdentity OK");

        CategoryLaws.Associativity(
                lawIntToString,
                lawStringToInt,
                lawIntToBool,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: 5));

        Console.WriteLine("Category.Associativity OK");


        Console.WriteLine("Probando Arrow laws...");

        ArrowLaws.LiftIdentity<
            Req<int, string>,
            int>(
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: 5));

        Console.WriteLine("Arrow.LiftIdentity OK");

        ArrowLaws.LiftComposition<
            Req<int, string>,
            int,
            string,
            int>(
                value => $"VALUE:{value}",
                value => value.Length,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: 5));

        Console.WriteLine("Arrow.LiftComposition OK");

        ArrowLaws.FirstIdentity<
            Req<int, string>,
            int,
            bool>(
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: (5, true)));

        Console.WriteLine("Arrow.FirstIdentity OK");

        ArrowLaws.FirstComposition<
            Req<int, string>,
            int,
            string,
            int,
            bool>(
                lawIntToString,
                lawStringToInt,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: (5, true)));

        Console.WriteLine("Arrow.FirstComposition OK");

        ArrowLaws.SecondDefinition<
            Req<int, string>,
            int,
            string,
            bool>(
                lawIntToString,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: (true, 5)));

        Console.WriteLine("Arrow.SecondDefinition OK");

        var lawBoolToInt =
            Arrow.Lift<Req<int, string>, bool, int>(value => value ? 1 : 0);

        ArrowLaws.SplitDefinition(
                lawIntToString,
                lawBoolToInt,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: (5, true)));

        Console.WriteLine("Arrow.SplitDefinition OK");

        var lawIntToBool2 =
            Arrow.Lift<Req<int, string>, int, bool>(value => value % 2 == 0);

        ArrowLaws.FanoutDefinition(
                lawIntToString,
                lawIntToBool2,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: 6));

        Console.WriteLine("Arrow.FanoutDefinition OK");

        var lawJoin =
            Arrow.Lift<Req<int, string>, (string Text, bool IsEven), string>(
                values => $"{values.Text}|EVEN:{values.IsEven}");

        ArrowLaws.ConvergeDefinition(
                lawIntToString,
                lawIntToBool2,
                lawJoin,
                (left, right) =>
                    Equivalent(
                        left,
                        right,
                        input: 42,
                        previous: 6));

        Console.WriteLine("Arrow.ConvergeDefinition OK");

        Console.WriteLine("Todas las leyes de Category y Arrow pasaron c:");
    }
}
