namespace Etutor.BL.Dtos.Identity
{
    public class RestorePasswordDto
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
