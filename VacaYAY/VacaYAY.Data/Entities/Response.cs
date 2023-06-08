﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacaYAY.Data.Enums;

namespace VacaYAY.Data.Entities;

public class Response
{
    [Key]
    public int ID { get; set; }

    [Required]
    public bool IsApproved { get; set; } = true;

    [NotMapped]
    public RequestStatus Status
    {
        get
        {
            return IsApproved ?
                RequestStatus.Approved
                :
                RequestStatus.Rejected;
        }
    }

    [MaxLength(256)]
    public string? Comment { get; set; }

    [Required]
    public int RequstID { get; set; }

    [Required]
    public Request Request { get; set; } = new();

    [Required]
    [DisplayName("Reviewed by")]
    public Employee ReviewedBy { get; set; } = new();
}
