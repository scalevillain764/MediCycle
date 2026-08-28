namespace Domain.Enums
{
    public enum WorkerRole { Admin, Dispatcher, Driver };
    public enum RequestStatus { Created, Assigned, InProgress, Completed, Cancelled }
    public enum ErrorType { Validation, Unauthorized, Forbidden, NotFound, Conflict }
}