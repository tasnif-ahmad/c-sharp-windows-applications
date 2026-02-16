using System;

namespace GoogleNegotiator.Models;

public class Replies
{
    public int Id { get; set; }
    public string UserText { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public DateTime ResponseDate { get; set; }
}
