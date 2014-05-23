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
        public DbSet<QuestionGroups> QuestionGroups { get; set; }
        public DbSet<Messages> Messages { get; set; }
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
        [Required(ErrorMessage = "DateTime is required")]
        public System.DateTime DateTime { get; set; }
        public string Votes { get; set; }
        public string Handled { get; set; }

        public virtual List<Messages> Messages { get; set; }
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

    [Table("Messages")]
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Message Text is required")]
        public string MessageText { get; set; }
        [Required(ErrorMessage = "DateTime is required")]
        public System.DateTime DateTime { get; set; }
    }
}