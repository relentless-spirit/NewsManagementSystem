﻿namespace NewsManagementSystem.Models;

public class CreateAccountRequest
{
    public short AccountID { get; set; }
    public string AccountName { get; set; }
    public string AccountEmail { get; set; }
    public int AccountRole { get; set; }
    public string AccountPassword { get; set; }
}