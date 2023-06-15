using VacaYAY.Data.Entities;

namespace VacaYAY.Data.Helpers;

public class RequestEmailTemplates
{
    private readonly Employee _employee;
    private readonly Request _request;

    public (string? email, string subject, string content) Created
    {
        get
        {
            string subject = "Leave Request Created";
            string message = "We wanted to inform you that your leave request has been successfully submited in our system.";

            return (_employee.Email, subject, GetEmployeeTemplate(message));
        }
    }

    public (string? email, string subject, string content) Deleted
    {
        get
        {
            string subject = "Leave Request Deleted";
            string message = "We wanted to inform you that your leave request has been successfully deleted from our system.";

            return (_employee.Email, subject, GetEmployeeTemplate(message));
        }
    }

    public (string? email, string subject, string content) Edited
    {
        get
        {
            string subject = "Leave Request Edited";
            string message = "We wanted to inform you that your leave request has been successfully edited in our system.";

            return (_employee.Email, subject, GetEmployeeTemplate(message));
        }
    }

    public (string? email, string subject, string content) Approved
    {
        get
        {
            string subject = "Leave Request Response";
            string message = "We wanted to inform you that your leave request has been approved by HR team.";

            return (_employee.Email, subject, GetEmployeeTemplate(message));
        }
    }

    public (string? email, string subject, string content) Rejected
    {
        get
        {
            string subject = "Leave Request Response";
            string message = "We wanted to inform you that your leave request has been rejected by HR team.";

            return (_employee.Email, subject, GetEmployeeTemplate(message));
        }
    }

    public (string subject, string content) HRCreated
    {
        get
        {
            string subject = $"Leave Request Notification: {_employee.Name}";

            return (subject, GetHRTemplate("created"));
        }
    }

    public (string subject, string content) HRDeleted
    {
        get
        {
            string subject = $"Leave Request Notification: {_employee.Name}";

            return (subject, GetHRTemplate("deleted"));
        }
    }

    public (string subject, string content) HREdited
    {
        get
        {
            string subject = $"Leave Request Notification: {_employee.Name}";

            return (subject, GetHRTemplate("edited"));
        }
    }

    private string GetHRTemplate(string action)
    {
        return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            font-size: 14px;
                        }}
                        table {{
                            border-collapse: collapse;
                            width: 100%;
                        }}
                        th, td {{
                            padding: 8px;
                            text-align: left;
                            border-bottom: 1px solid #ddd;
                        }}
                        th {{
                            background-color: #f2f2f2;
                        }}
                    </style>
                </head>
                <body>
                    <h2>Leave Request Notification: {_employee.Name}</h2>
                    <p><strong>{_employee.Name}</strong> has {action} a <a href=""https://localhost:7105/Requests/Details/{_request.ID}"">leave request</a>.</p>
                    <table>
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                        <tr>
                            <td>Employee</td>
                            <td>{_employee.Name}</td>
                        </tr>
                        <tr>
                            <td>Leave start date</td>
                            <td>{_request.StartDate.ToString("dd.MM.yyyy")}</td>
                        </tr>
                        <tr>
                            <td>Leave end date</td>
                            <td>{_request.EndDate.ToString("dd.MM.yyyy")}</td>
                        </tr>
                        <tr>
                            <td>Number of leave days requested</td>
                            <td>{_request.NumOfDaysRequested}</td>
                        </tr>
                        <tr>
                            <td>Leave type</td>
                            <td>{_request.LeaveType.Caption}</td>
                        </tr>
                        <tr>
                            <td>Comment</td>
                            <td>{_request.Comment ?? "No comment provided."}</td>
                        </tr>
                    </table>
                </body>
                </html>
                ";
    }

    private string GetEmployeeTemplate(string message)
    {
        return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        font-size: 14px;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                    }}
                    .message {{
                        margin-bottom: 20px;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <h2>Leave Request Update Notification</h2>
                    <div class=""message"">
                        <p>Dear {_employee.Name},</p>
                        <p>{message}</p>
                    </div>
                    <div class=""message"">
                        <p>If you have any further questions or require assistance, please don't hesitate to reach out to our HR department.</p>
                        <p>Best regards,</p>
                        <p>Your HR team</p>
                    </div>
                </div>
            </body>
            </html>
            ";
    }

    public RequestEmailTemplates(
        Employee employee,
        Request request)
    {
        _employee = employee;
        _request = request;
    }
}
