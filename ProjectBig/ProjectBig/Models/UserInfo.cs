using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

public class UserInfo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    public string fname { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    public string lname { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    public string username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string email { get; set; }

    [Required(ErrorMessage = "Age is required.")]
    [Range(18, int.MaxValue, ErrorMessage = "Age must be 18 or older.")]
    public int age { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    public string phoneno { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string password { get; set; }

    [Display(Name = "Profile Picture")]
    public string ProfilePicture { get; set; }

    public int isadmin { get; set; }
}
