﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Entities;

public class Request
{
    [Key]
    public int ID { get; set; }

    [Required]
    [DisplayName("Start date")]
    public DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End date")]
    public DateTime EndDate { get; set; }

    [MaxLength(256)]
    public string? Comment { get; set; }

    [Required]
    [DisplayName("Leave type")]
    public LeaveType LeaveType { get; set; } = new();

    [Required]
    [DisplayName("Requested by")]
    public Employee CreatedBy { get; set; } = new();

    public Response? Response { get; set; }
}