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
            var content = $"Dear {_employee.Name},\n\n" +
                $"Thank you for submitting your leave request. " +
                $"We have received your request and it is currently being reviewed by our HR team. " +
                $"We will notify you of the status of your request as soon as possible.\n\n" +
                $"If you have any questions or need further assistance, please don't hesitate to reach out to our HR department.\n\n" +
                $"Best regards," +
                $"\nYour HR team.";

            return (_employee.Email, subject, content);
        }
    }

    public (string? email, string subject, string content) Deleted
    {
        get
        {
            string subject = "Leave Request Deleted";
            var content = $"Dear {_employee.Name},\n\n" +
                $"We wanted to inform you that your leave request has been successfully deleted from our system.\n\n" +
                $"If you have any further questions or require assistance, please don't hesitate to reach out to our HR department.\n\n" +
                $"Best regards,\n" +
                $"Your HR team.";

            return (_employee.Email, subject, content);
        }
    }

    public (string? email, string subject, string content) Edited
    {
        get
        {
            string subject = "Leave Request Edited";
            var content = $"Dear {_employee.Name},\n\n" +
                $"We wanted to inform you that your leave request has been successfully updated in our system.\n\n" +
                $"If you have any further questions or require assistance, please don't hesitate to reach out to our HR department.\n\n" +
                $"Best regards,\n" +
                $"Your HR team.";

            return (_employee.Email, subject, content);
        }
    }

    public (string subject, string content) HRCreated
    {
        get
        {
            string subject = $"Leave Request Notification: {_employee.Name}";
            string content = $"{_employee.Name} has submitted a leave request.\r\n\r\n" +
                $"Employee Name: {_employee.Name}\r\n" +
                $"Leave Start Date: {_request.StartDate.ToString("dd.MM.yyyy")}\r\n" +
                $"Leave End Date: {_request.EndDate}\r\n" +
                $"Number of Leave Days: {_request.NumOfDaysRequested}\r\n" +
                $"Type of Leave: {_request.LeaveType.Caption}\r\n" +
                $"Comment: {_request.Comment}\r\n\r\n";

            return (subject, content);
        }
    }

    public (string subject, string content) HRDeleted
    {
        get
        {
            string subject = $"Leave Request Notification: {_employee.Name}";
            string content = $"{_employee.Name} has deleted a leave request.\r\n\r\n" +
                $"Employee Name: {_employee.Name}\r\n" +
                $"Leave Start Date: {_request.StartDate.ToString("dd.MM.yyyy")}\r\n" +
                $"Leave End Date: {_request.EndDate}\r\n" +
                $"Number of Leave Days: {_request.NumOfDaysRequested}\r\n" +
                $"Type of Leave: {_request.LeaveType.Caption}\r\n" +
                $"Comment: {_request.Comment}\r\n\r\n";

            return (subject, content);
        }
    }

    public (string subject, string content) HREdited
    {
        get
        {
            string subject = $"Leave Request Notification: {_employee.Name}";
            string content = $"{_employee.Name} has edited a leave request.\r\n\r\n" +
                $"Employee Name: {_employee.Name}\r\n" +
                $"Leave Start Date: {_request.StartDate.ToString("dd.MM.yyyy")}\r\n" +
                $"Leave End Date: {_request.EndDate}\r\n" +
                $"Number of Leave Days: {_request.NumOfDaysRequested}\r\n" +
                $"Type of Leave: {_request.LeaveType.Caption}\r\n" +
                $"Comment: {_request.Comment}\r\n\r\n";

            return (subject, content);
        }
    }


    public RequestEmailTemplates(
        Employee employee,
        Request request)
    {
        _employee = employee;
        _request = request;
    }
}
