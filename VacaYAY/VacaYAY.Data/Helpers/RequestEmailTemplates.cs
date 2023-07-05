using VacaYAY.Data.Entities;

namespace VacaYAY.Data.Helpers;

public enum EmailTemplateType
{
    Created,
    Edited,
    Deleted,
    Approved,
    Rejected,
    CollectiveVacation
}

public static class RequestEmailTemplates
{
    public static (string? email, string subject, string content) GetEmail(
        EmailTemplateType emailType,
        Employee employee,
        Request request,
        bool isForHR = false) => (emailType, isForHR) switch
        {
            (EmailTemplateType.CollectiveVacation, false) => (employee.Email, "Collective vacation", GetContentForCollectiveVacation(employee, request)),
            (EmailTemplateType.Approved, false) => (employee.Email, "Leave Request Response", GetContentForResponse(emailType, employee, request)),
            (EmailTemplateType.Rejected, false) => (employee.Email, "Leave Request Response", GetContentForResponse(emailType, employee, request)),
            (_, true) => (null, $"Leave Request Notification: {employee.Name}", GetContentForRequest(emailType, employee, request, isForHR: true)),
            (_, _) => (employee.Email, $"Leave Request {emailType}", GetContentForRequest(emailType, employee, request))
        };

    private static string GetContentForRequest(
        EmailTemplateType emailType,
        Employee employee,
        Request request,
        bool isForHR = false)
    {
        string content = $@"
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
                        <p>Dear {employee.Name},</p>
                        <p>We wanted to inform you that your leave request has been successfully {emailType.ToString().ToLower()} in our system.</p>
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

        string hrContent = $@"
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
                    <h2>Leave Request Notification: {employee.Name}</h2>
                    <p><strong>{employee.Name}</strong> has {emailType.ToString().ToLower()} a <a href=""https://localhost:7105/Requests/Details/{request.ID}"">leave request</a>.</p>
                    <table>
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                        <tr>
                            <td>Employee</td>
                            <td>{employee.Name}</td>
                        </tr>
                        <tr>
                            <td>Leave start date</td>
                            <td>{request.StartDate.ToString("dd.MM.yyyy")}</td>
                        </tr>
                        <tr>
                            <td>Leave end date</td>
                            <td>{request.EndDate.ToString("dd.MM.yyyy")}</td>
                        </tr>
                        <tr>
                            <td>Number of leave days requested</td>
                            <td>{request.NumOfDaysRequested}</td>
                        </tr>
                        <tr>
                            <td>Leave type</td>
                            <td>{request.LeaveType.Caption}</td>
                        </tr>
                        <tr>
                            <td>Comment</td>
                            <td>{request.Comment ?? "No comment provided."}</td>
                        </tr>
                    </table>
                </body>
                </html>
                ";

        return isForHR ? hrContent : content;
    }

    private static string GetContentForResponse(
        EmailTemplateType type,
        Employee employee,
        Request request)
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
                    <div class=""container"">
                    <h2>Response for submitted leave request</h2>
                    <div class=""message"">
                        <p>Dear {employee.Name},</p>
                        <p>We wanted to inform you that your leave request has been {type.ToString().ToLower()} by HR team.</p>
                    <table>
                        <tr>
                            <th>Field</th>
                            <th>Value</th>
                        </tr>
                        <tr>
                            <td>Leave start date</td>
                            <td>{request.StartDate.ToString("dd.MM.yyyy")}</td>
                        </tr>
                        <tr>
                            <td>Leave end date</td>
                            <td>{request.EndDate.ToString("dd.MM.yyyy")}</td>
                        </tr>
                        <tr>
                            <td>Number of leave days requested</td>
                            <td>{request.NumOfDaysRequested}</td>
                        </tr>
                        <tr>
                            <td>Leave type</td>
                            <td>{request.LeaveType.Caption}</td>
                        </tr>
                    </table>
                    <div class=""message"">
                        <p> You have <strong>{employee.DaysOffNumber}</strong> free days left. </p>
                        <p>If you have any further questions or require assistance, please don't hesitate to reach out to our HR department.</p>
                        <p>Best regards,</p>
                        <p>Your HR team</p>
                    </div>
                    </div>
                </body>
                </html>
                ";
    }

    private static string GetContentForCollectiveVacation(
        Employee employee,
        Request request)
    {
        string content = $@"
                <!DOCTYPE html>  
                <html>
                <head>
                    <meta charset=""UTF-8"">
                    <title>Collective Vacation Announcement</title>
                </head>
                <body style=""font-family: Arial, sans-serif;"">

                    <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px; margin: 0 auto; border-collapse: collapse;"">
                    <tr>
                        <td bgcolor=""#ffffff"" style=""padding: 40px 30px;"">
                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                            <tr>
                            <td style=""padding-top: 20px;"">
                                <p>Dear {employee.Name},</p>
                                <p>We are pleased to announce that a collective vacation has been scheduled for all employees at our company. This vacation period will allow everyone to take a well-deserved break and recharge.</p>
                                <p>Here are the details of the collective vacation:</p>
                                <ul>
                                <li>Start Date: {request.StartDate.ToString("dd. MM. yyyy.")}</li>
                                <li>End Date: {request.EndDate.ToString("dd. MM. yyyy.")}</li>
                                <li>Duration: {request.NumOfDaysRequested} days</li>
                                </ul>
                                <p>Please note that during this period, all employees are expected to take time off and refrain from work-related activities. We encourage you to fully disconnect from work and enjoy your vacation.</p>
                                <p>If you have any questions or concerns regarding the collective vacation, please feel free to reach out to the Human Resources department.</p>
                                <p>Thank you for your understanding and cooperation.</p>
                                <p>Best regards,</p>
                                <p>Your HR team</p>
                            </td>
                            </tr>
                        </table>
                        </td>
                    </tr>
                    </table>

                </body>
                </html>";
        return content;
    }
}
