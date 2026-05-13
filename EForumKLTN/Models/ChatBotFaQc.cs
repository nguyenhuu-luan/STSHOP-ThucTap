using System;
using System.Collections.Generic;

namespace EForumKLTN.Models;

public partial class ChatBotFaQ
{
    public int Id { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? NoiDungTraLoi { get; set; }

    public int? ParentId { get; set; }

    public bool IsMenu { get; set; }

    public int ThuTu { get; set; }

    public bool IsActive { get; set; }

    public DateTime? NgayTao { get; set; }
}