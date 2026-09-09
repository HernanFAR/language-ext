using ArrowTests.Monads;
using LanguageExt;
using System.Diagnostics;

using static ArrowTests.Monads.Req<string, ArrowTests.ValueObjects.StreetExtension>;


namespace ArrowTests.ValueObjects;

public sealed class StreetExtension
{
    public const string EmptyMsg = "Debes especificar la extensión";

    public const string InvalidLengthMsg = "Debe tener entre 3 y 16 caracteres";

    public const string InvalidStructureMsg = "Debes especificar un nombre y un valor, separados por espacio";

    private readonly string _name;
    private readonly string _value;

    public static Req<string, StreetExtension, string, StreetExtension> Invariants =>
        Avoid((string v) => string.IsNullOrWhiteSpace(v), Fail: EmptyMsg)
        >> Ensure((string v) => v.Length is >= 3 and <= 16, Fail: InvalidLengthMsg)
        * (i => i.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        >> Ensure((string[] v) => v.Length > 1, Fail: InvalidStructureMsg)
        * New;

    private static StreetExtension New(string[] i) =>
        i is [var a, .. var b]
            ? new StreetExtension(a, string.Join(" ", b))
            : throw new UnreachableException("No deberia pasar esto");

    private StreetExtension(string name, string value) =>
        (_name, _value) = (name, value);

    public override string ToString() =>
        $"{_name} {_value}";

    public string To() => ToString();

    public static Fin<StreetExtension> Create(string value) =>
        Invariants.RunFin(value);

    public static Fin<StreetExtension> CreateStreetNumber(string value) =>
        StreetExtension.Create($"N° {value}");

    public static Fin<StreetExtension> CreateDepto(string value) =>
        StreetExtension.Create($"Depto {value}");

    public static Fin<StreetExtension> CreateTower(string value) =>
        StreetExtension.Create($"Torre {value}");

    public static Fin<StreetExtension> CreateBlock(string value) =>
        StreetExtension.Create($"Bloque {value}");

    public static Fin<StreetExtension> CreateSector(string value) =>
        StreetExtension.Create($"Sector {value}");

    public static Fin<StreetExtension> CreateBuilding(string value) =>
        StreetExtension.Create($"Edificio {value}");

    public static Fin<StreetExtension> CreateParcel(string value) =>
        StreetExtension.Create($"Parcela {value}");
}
