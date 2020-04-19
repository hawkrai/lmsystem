using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LMPlatform.UI.ViewModels.AdministrationViewModels;

namespace LMPlatform.UI.Attributes
{
  //public class PasswordRequiredIfResetAttribute : ValidationAttribute
  //{
  //  public PasswordRequiredIfResetAttribute()
  //  {
  //    ErrorMessage = "Для сброса пароля нужно ввести и подтвердить новый пароль";
  //  }

  //  public override bool IsValid(object value)
  //  {
  //    var viewModel = value as ModifyStudentViewModel;
  //    if (viewModel == null || !viewModel.IsPasswordReset)
  //    {
  //      return true;
  //    }

  //    return viewModel.IsPasswordReset && !string.IsNullOrWhiteSpace(viewModel.Password);
  //  }
  //}
}