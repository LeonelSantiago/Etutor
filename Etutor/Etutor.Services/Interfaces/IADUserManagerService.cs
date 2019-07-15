using System.Collections;
using System.DirectoryServices.AccountManagement;
using Etutor.Core.Models.Configurations;

namespace Etutor.Services.Interfaces
{
    public interface IADUserManagerService
    {
        #region Props
        AdConfig Config { get; }
        #endregion

        #region Validate Methods
        bool ValidateCredentials(string sUserName, string sPassword);
        bool IsUserExpired(string sUserName);
        bool IsUserExisiting(string sUserName);
        bool IsAccountLocked(string sUserName);
        #endregion

        #region Search Methods
        UserPrincipal GetUser(string sUserName);
        GroupPrincipal GetGroup(string sGroupName);
        #endregion

        #region User Account Methods
        bool SetUserPassword(string sUserName, string sNewPassword);
        void EnableUserAccount(string sUserName);
        void DisableUserAccount(string sUserName);
        void ExpireUserPassword(string sUserName);
        void UnlockUserAccount(string sUserName);
        bool DeleteUser(string sUserName);
        UserPrincipal CreateNewUser(string sOU, string sUserName, string sPassword, string sGivenName, string sSurname);
        #endregion

        #region Group Methods
        GroupPrincipal CreateNewGroup(string sOU, string sGroupName, string sDescription, GroupScope oGroupScope, bool bSecurityGroup);
        bool AddUserToGroup(string sUserName, string sGroupName);
        bool RemoveUserFromGroup(string sUserName, string sGroupName);
        bool IsUserGroupMember(string sUserName, string sGroupName);
        ArrayList GetUserGroups(string sUserName);
        ArrayList GetUserAuthorizationGroups(string sUserName);
        object GetADProperty(string sUserName, string property);
        #endregion

    }
}
