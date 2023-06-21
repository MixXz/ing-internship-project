﻿using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts.ServiceContracts;

public interface INotifierSerivice
{
    Task<bool> NotifyEmployee((string? email, string subject, string content) message);
    Task NotifyHRTeam((string subject, string content) message);
}