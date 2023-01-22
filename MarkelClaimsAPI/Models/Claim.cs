using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MarkelClaimsAPI.Models;

public partial class Claim
{
    public string Ucr { get; set; } = null!;

    public int? CompanyId { get; set; }

    public DateTime? ClaimDate { get; set; }

    public DateTime? LossDate { get; set; }

    public string? AssuredName { get; set; }

    public decimal? IncurredLoss { get; set; }

    public bool? Closed { get; set; }

    [JsonIgnore]
    public virtual Company? Company { get; set; }
}
