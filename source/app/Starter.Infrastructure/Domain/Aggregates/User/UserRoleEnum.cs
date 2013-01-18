using System.ComponentModel;
namespace DQF.Domain.Aggregates.User
{
    public enum UserRoleEnum
    {
        [Description("Admin")]
        GlobalAdmin,
        [Description("User")]
        User,
    }
}