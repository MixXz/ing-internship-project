﻿using System.ComponentModel.DataAnnotations;

namespace VacaYAY.API.Entities;

public class Position
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Caption { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;
}
