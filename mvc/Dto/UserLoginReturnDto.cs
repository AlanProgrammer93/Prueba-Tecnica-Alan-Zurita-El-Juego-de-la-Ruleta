using mvc.Models;

namespace mvc.Dto
{
    public class UserLoginReturnDto
    {
        public User user { get; set; }
        public string token { get; set; }
    }
}
