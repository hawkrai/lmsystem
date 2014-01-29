using System.ComponentModel;

namespace Application.Core.Enums
{
    public enum Roles
    {
        [Description(Constants.Constants.Roles.Admin)]
        Admin,
        [Description(Constants.Constants.Roles.Lector)]
        Lector,
        [Description(Constants.Constants.Roles.Student)]
        Student
    }
}
