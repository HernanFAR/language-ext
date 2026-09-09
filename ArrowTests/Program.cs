
using System.Diagnostics;
using ArrowTests.Monads;
using ArrowTests.ValueObjects;
using LanguageExt;
using static LanguageExt.Prelude;


ReqLawTesting.TestLaws();

_ = StreetExtension.Create("") is Fin<StreetExtension>.Fail(var m1Error)
    ? unit : throw new Exception();

_ = m1Error.Message is "[Debes especificar la extensión, Debe tener entre 3 y 16 caracteres]"
    ? unit : throw new Exception();

_ = StreetExtension.Create("12") is Fin<StreetExtension>.Fail(var m2Error)
    ? unit : throw new Exception();

_ = m2Error.Message is "Debe tener entre 3 y 16 caracteres"
    ? unit : throw new Exception();

_ = StreetExtension.Create("12345678901234567") is Fin<StreetExtension>.Fail(var m3Error)
    ? unit : throw new Exception();

_ = m3Error.Message is "Debe tener entre 3 y 16 caracteres"
    ? unit : throw new Exception();

_ = StreetExtension.Create("1234") is Fin<StreetExtension>.Fail(var m4Error)
    ? unit : throw new Exception();

_ = m4Error.Message is "Debes especificar un nombre y un valor, separados por espacio"
    ? unit : throw new Exception();

_ = StreetExtension.Create(" 1234") is Fin<StreetExtension>.Fail(var m5Error)
    ? unit : throw new Exception();

_ = m4Error.Message is "Debes especificar un nombre y un valor, separados por espacio"
    ? unit : throw new Exception();

_ = StreetExtension.Create("1234 ") is Fin<StreetExtension>.Fail(var m6Error)
    ? unit : throw new Exception();

_ = m6Error.Message is "Debes especificar un nombre y un valor, separados por espacio"
    ? unit : throw new Exception();

_ = StreetExtension.Create("Depto 2309B") is Fin<StreetExtension>.Succ(var value)
    ? unit : throw new Exception();

_ = value.ToString() is "Depto 2309B" ? unit : throw new Exception();

Console.WriteLine($"Exito con las cosas de extensión: {value}");

