using Etutor.BL.Abstract;

namespace Etutor.BL.Dtos
{
    public class UserDto : IEntityBaseDto
    {
        #region Ctor
        public UserDto()
        {

        }
        #endregion

        #region Properties
        public int Id { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        /*Identity fields*/
        public virtual string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        /*End Identity fields*/
        #endregion
    }
}
