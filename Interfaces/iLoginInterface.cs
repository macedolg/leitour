public interface iLoginView
{
    [Required]
    [EmailAddress]
    string Email { get; set; }

    [Required]
    [MinLength(8)]
    string Password { get; set; }

    bool RememberMe { get; set; }
}