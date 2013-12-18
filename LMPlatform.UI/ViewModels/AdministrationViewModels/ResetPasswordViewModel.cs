using System.ComponentModel.DataAnnotations;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
  public class ResetPasswordViewModel
  {
    public ResetPasswordViewModel(User user)
    {
      FullName = user.Student != null ? user.Student.FullName : user.Lecturer.FullName;
      
      Login = user.UserName;
    }

    public ResetPasswordViewModel()
    {
    }

    public string FullName { get; set; }

    [Required]
    public string Login { get; set; }

    [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
    [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "Пароль и подтвержденный пароль не совпадают.")]
    public string ConfirmPassword { get; set; }

    public bool ResetPassword()
    {
      var token = WebSecurity.GeneratePasswordResetToken(Login, 1);
      var isReseted = WebSecurity.ResetPassword(token, Password);
      return isReseted;
    }
  }
}