using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;
using System.Xml.Linq;

namespace Content.Shared.Roles;

[Prototype("department")]
public sealed class DepartmentPrototype : IPrototype
{
    [IdDataFieldAttribute] public string ID { get; } = default!;

    /// <summary>
    ///     A description string to display in the character menu as an explanation of the department's function.
    /// </summary>
    [DataField("description", required: true)]
    public string Description = default!;

    /// <summary>
    ///     A color representing this department to use for text.
    /// </summary>
    [DataField("color", required: true)]
    public Color Color = default!;

    [ViewVariables(VVAccess.ReadWrite),
     DataField("roles", customTypeSerializer: typeof(PrototypeIdListSerializer<JobPrototype>))]
    public List<string> Roles = new();

    [DataField("accountNumber")]
    public int? AccountNumber { get; }
}
