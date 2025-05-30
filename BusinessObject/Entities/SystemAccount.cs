﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Entities;

[Table("SystemAccount")]
public partial class SystemAccount
{
    [Key]
    public short AccountID { get; set; }

    [StringLength(100)]
    public string AccountName { get; set; }

    [StringLength(70)]
    public string AccountEmail { get; set; }

    public int? AccountRole { get; set; }

    [StringLength(70)]
    public string AccountPassword { get; set; }

    [InverseProperty("CreatedBy")]
    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}