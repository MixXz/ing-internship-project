using VacaYAY.Data.Entities;

namespace VacaYAY.Data.Helpers;

public class EmployeeEmailTemplates
{
    private Employee _employee;
    private IEnumerable<Employee> _employees;

    public Employee Employee
    {
        set
        {
            _employee = value;
        }
    }
    public IEnumerable<Employee> ListOfEmployees
    {
        set
        {
            _employees = value;
        }
    }

    public (string? emailTo, string subject, string content) RemainingDaysOff
    {
        get
        {
            string subject = "Reminder: Utilize Remaining Vacation Days";
            string content = $@"<!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        line-height: 1.5;
                        margin: 0;
                        padding: 0;
                        background-color: #f5f5f5;
                    }}
        
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        background-color: #ffffff;
                        border-radius: 5px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    }}
        
                    h2 {{
                        color: #333333;
                    }}
        
                    p {{
                        color: #666666;
                    }}
        
                    .button {{
                        display: inline-block;
                        margin-top: 10px;
                        padding: 10px 20px;
                        background-color: #007bff;
                        text-decoration: none;
                        color: #ffffff;
                        border-radius: 4px;
                        transition: background-color 0.3s ease;
                    }}
        
                    .button:hover {{
                        background-color: #0056b3;
                        color: #ffffff;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <h2>Utilize Your Remaining Vacation Days</h2>
                    <p>Dear {_employee.Name},</p>
                    <p>This is a friendly reminder that you have <strong>{_employee.DaysOffNumber}</strong> vacation days remaining for this year. We encourage you to plan and use them to ensure a healthy work-life balance.</p>
                    <p>To schedule your time off, please refer to the company's vacation policy or consult with your supervisor for any guidelines. Remember, unutilized vacation days cannot be carried forward.</p>
                    <div class=""message"">
                        <p>If you have any further questions or require assistance, please don't hesitate to reach out to our HR department.</p>
                        <p>Best regards,</p>
                        <p>Your HR team</p>
                    </div>
                    <div>
                        <a href=""https://localhost:7105/Requests/CreateRequest"" class=""button"">Create leave request</a>
                    </div>
                </div>
            </body>
            </html>";


            return (_employee.Email, subject, content);
        }
    }

    public (string subject, string content) RemainingDaysOffHR
    {
        get
        {
            string subject = "Remaining days off for employees";
            string content = $@"<!DOCTYPE html>
                        <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    line-height: 1.5;
                                    margin: 0;
                                    padding: 0;
                                    background-color: #f5f5f5;
                                }}
                                
                                h2 {{
                                    color: #333333;
                                }}
                            </style>
                        </head>
                        <body>
                            <div style='max-width: 600px; margin: 20px auto; padding: 20px; background-color: #ffffff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);'>
                                <h2>Employees with remaining days off</h2>
                                <p>Below is the list of employees who have remaining days off:</p>
                                {GetEmployeeTable()}
                            </div>
                        </body>
                        </html>";

            return (subject, content);
        }
    }

    public (string subject, string content) RemovedOldDaysOffHR
    {
        get
        {
            string subject = "Removed old days off for employees";
            string content = $@"<!DOCTYPE html>
                        <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    line-height: 1.5;
                                    margin: 0;
                                    padding: 0;
                                    background-color: #f5f5f5;
                                }}
                                
                                h2 {{
                                    color: #333333;
                                }}
                            </style>
                        </head>
                        <body>
                            <div style='max-width: 600px; margin: 20px auto; padding: 20px; background-color: #ffffff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);'>
                                <h2>Employees who had remaining days off</h2>
                                {GetEmployeeTable()}
                            </div>
                        </body>
                        </html>";

            return (subject, content);
        }
    }

    private string GetEmployeeTable()
    {
        string tableContent = $@"<table style='width: 100%; border-collapse: collapse;'>
                            <thead>
                                <tr>
                                    <th style='padding: 10px; background-color: #f2f2f2; border-bottom: 1px solid #ddd;'>Employee Name</th>
                                    <th style='padding: 10px; background-color: #f2f2f2; border-bottom: 1px solid #ddd;'>Days Left</th>
                                </tr>
                            </thead>
                            <tbody>";

        foreach (var employee in _employees)
        {
            tableContent += $@"<tr>
                            <td style='padding: 10px; border-bottom: 1px solid #ddd;'>{employee.Name}</td>
                            <td style='padding: 10px; border-bottom: 1px solid #ddd;'>{employee.DaysOffNumber}</td>
                       </tr>";
        }

        tableContent += "</tbody></table>";

        return tableContent;
    }
    public EmployeeEmailTemplates()
    {
        _employee = new();
        _employees = Enumerable.Empty<Employee>();
    }
}
