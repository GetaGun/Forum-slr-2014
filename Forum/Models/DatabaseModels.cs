using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Web.Helpers;
using Forum.Models;
using System.ComponentModel;

namespace Forum.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Votes> Votes { get; set; }
        public DbSet<QuestionGroups> QuestionGroups { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
    
    [Table("Questions")]
    public class Questions
    {
        [Key]
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Question Group is required")]
        public int QuestionGroupId { get; set; }
        [Required(ErrorMessage = "Question Name is required")]
        public string QuestionName { get; set; }
        [Required(ErrorMessage = "Question Description is required")]
        public string QuestionDescription { get; set; }
	    [Required(ErrorMessage = "Question Keyword is required")]
        public string QuestionKeyword { get; set; }
        [Required(ErrorMessage = "DateTime is required")]
        public System.DateTime Date { get; set; }
        
        public virtual List<Messages> Messages { get; set; }
        public virtual List<Votes> Votes { get; set; }
        public virtual QuestionGroups QuestionGroups { get; set; }
    }

    [Table("QuestionGroups")]
    public class QuestionGroups
    {
        [Key]
        public int QuestionGroupId { get; set; }
        [Required(ErrorMessage = "Question Group is required")]
        public string QuestionGroupName { get; set; }

        public virtual List<Questions> Questions { get; set; }
    }

    [Table("Votes")]
    public class Votes
    {
        [Key]
        public int VoteId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
    }

    [Table("Messages")]
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Message Text is required")]
        public string MessageText { get; set; }
        [Required(ErrorMessage = "DateTime is required")]
        public System.DateTime Date { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}