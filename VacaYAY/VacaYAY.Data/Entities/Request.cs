﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacaYAY.Data.Enums;

namespace VacaYAY.Data.Entities;

public class Request
{
    [Key]
    public int ID { get; set; }

    [Required]
    [DisplayName("Start date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [NotMapped]
    [DisplayName("Number of days requested")]
    public int NumOfDaysRequested
    {
        get
        {
            return (int)(EndDate - StartDate).TotalDays;
        }
    }

    [NotMapped]
    public RequestStatus Status
    {
        get
        {
            if (Response is null)
            {
                return RequestStatus.Pending;
            }
            else if (Response.IsApproved)
            {
                return RequestStatus.Approved;
            }

            return RequestStatus.Rejected;
        }
    }

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