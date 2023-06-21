﻿using Microsoft.AspNetCore.Identity;
using VacaYAY.Business.Contracts;
using VacaYAY.Business.Contracts.RepositoryContracts;
using VacaYAY.Business.Contracts.ServiceContracts;
using VacaYAY.Business.Repository;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Services;
public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;
    private readonly UserManager<Employee> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserStore<Employee> _userStore;
    private readonly IBlobService _blobService;

    private EmployeeRepository? _employeeRepository;
    private RequestRepository? _requestRepository;
    private ResponseRepository? _responseRepository;
    private PositionRepository? _positionRepository;
    private LeaveTypeRepository? _leaveTypeRepository;
    private ContractRepository? _contractRepository;

    public IEmployeeRepository Employee
    {
        get
        {
            if (_employeeRepository == null)
            {
                _employeeRepository = new(
                    _context,
                    _userManager,
                    _roleManager,
                    _userStore);
            }

            return _employeeRepository;
        }
    }

    public IRequestRepository Request
    {
        get
        {
            if (_requestRepository == null)
            {
                _requestRepository = new(_context);
            }

            return _requestRepository;
        }
    }

    public IResponseRepository Response
    {
        get
        {
            if (_responseRepository == null)
            {
                _responseRepository = new(_context);
            }

            return _responseRepository;
        }
    }

    public IPositionRepository Position
    {
        get
        {
            if (_positionRepository == null)
            {
                _positionRepository = new(_context);
            }

            return _positionRepository;
        }
    }

    public ILeaveTypeRepository LeaveType
    {
        get
        {
            if (_leaveTypeRepository == null)
            {
                _leaveTypeRepository = new(_context);
            }

            return _leaveTypeRepository;
        }
    }

    public IContractRepository Contract
    {
        get
        {
            if (_contractRepository == null)
            {
                _contractRepository = new(
                    _context,
                    _blobService);
            }

            return _contractRepository;
        }
    }


    public UnitOfWork(
        Context context,
        UserManager<Employee> userManager,
        RoleManager<IdentityRole> roleManager,
        IUserStore<Employee> userStore,
        IBlobService blobService
        )
    {
        _context = context;
        _roleManager = roleManager;
        _userStore = userStore;
        _userManager = userManager;
        _blobService = blobService;
    }
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}