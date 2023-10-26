public class LoginModel : iLoginView
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}